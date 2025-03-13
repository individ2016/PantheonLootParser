using HtmlAgilityPack;

namespace PantheonLootParser
{
	public class ShalazamParser
	{
		private TimeSpan dbCacheSpan = TimeSpan.FromDays(7);
		private DAL _DAL;
		private List<DataItem> _items;

		private Dictionary<String, String> _autoReplacements = new Dictionary<String, String>()
		{
			{ "&centerdot;", "·" },
		};

		public ShalazamParser()
		{
			_DAL = new DAL();
			_DAL.EnsureDBCreated();
		}

		protected async Task EnsureItemsList()
		{
			if(_items != null)
				return;

			using(HttpClient client = new HttpClient())
			{
				String page = await client.GetStringAsync("https://shalazam.info/items/1/compare");
				var doc = new HtmlAgilityPack.HtmlDocument();
				doc.LoadHtml(page);

				var selectElement = doc.DocumentNode.SelectSingleNode("//select[@id='base_id']");
				if(selectElement != null)
				{
					_items = new List<DataItem>();
					// Находим все дочерние элементы <option>
					foreach(var option in selectElement.SelectNodes(".//option"))
					{
						string idString = option.GetAttributeValue("value", "N/A");
						string text = option.InnerText.Trim();

						if (Int32.TryParse(idString, out Int32 itemId))
							_items.Add(new DataItem() { ID = itemId, Name = text });
					}
				}
			}
		}

		public async Task<String> GetItemFromDB(DataItem item)
		{
			String result = String.Empty;
			await foreach(var attributeItem in _DAL.GetItemAttributes(item.ID))
			{
				if(!String.IsNullOrEmpty(result))
					result += "\\n";
				if(attributeItem.Value != null)
					result += $"{attributeItem.Key}: {attributeItem.Value}";
				else
					result += attributeItem.Key;
			}
			return MakeAutoReplacements(result);
		}

		public async Task<String> GetItem(DataItem item)
		{
			String result = String.Empty;

			DataItem dbItem = await _DAL.GetItem(item.ID);
			if(dbItem?.InsDate?.Add(dbCacheSpan) > DateTime.Now)
				return await GetItemFromDB(dbItem);
			else
				_DAL.RemoveItem(item.ID);

			using(HttpClient client = new HttpClient())
			{
				String page = await client.GetStringAsync($"https://shalazam.info/items/{item.ID}/compare");
				var doc = new HtmlAgilityPack.HtmlDocument();
				doc.LoadHtml(page);

				var tableElement = doc.DocumentNode.SelectSingleNode("//table[@class='item-comparison']");
				if(tableElement != null)
				{
					foreach(var row in tableElement.SelectNodes(".//tr[not(@aria-hidden)]"))
					{
						HtmlNode leftNode = row.SelectSingleNode(".//div[@class='level-left']");
						if (leftNode != null)
						{
							HtmlNode rightNode = row.SelectSingleNode(".//div[@class='level-right']");
							if(rightNode != null)
								_DAL.AddAttributeItem(item.ID, leftNode.InnerText.Trim(), rightNode.InnerText.Trim());
						} else
							_DAL.AddAttributeItem(item.ID, row.InnerText.Trim());
					}
					_DAL.AddItem(item.ID, item.Name);
					return await GetItemFromDB(item);
				}
			}
			return String.Empty;
		}

		private String MakeAutoReplacements(String input)
		{
			foreach(String key in _autoReplacements.Keys)
				if (input.Contains(key))
					input = input.Replace(key, _autoReplacements[key]);
			return input;
		}

		public async Task<DataItem> GetItem(String itemTitle)
		{
			await EnsureItemsList();
			Int32 lastDistance = Int32.MaxValue;
			DataItem foundItem = null;
			foreach (var item in _items)
			{
				Int32 itemDistance = TextUtils.DamerauLevenshteinDistance(item.Name, itemTitle, Int32.MaxValue);
				if (itemDistance < lastDistance)
				{
					lastDistance = itemDistance;
					foundItem = item;
				}
			}

			if(lastDistance <= 3)
				return foundItem;

			return null;
		}
	}
}
