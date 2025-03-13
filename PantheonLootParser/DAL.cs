using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantheonLootParser
{
	public class DataItem
	{
		public Int32 ID { get; set; }
		public string Name { get; set; }
		public DateTime? InsDate { get; set; }
	}

	internal class DAL
	{
		const string ConnectionString = "Data Source=descriptions.db";
		public DAL() {
		}

		public async void EnsureDBCreated()
		{
			using(var connection = new SqliteConnection(ConnectionString))
			{
				connection.Open();
				using(var command = connection.CreateCommand())
				{
					command.CommandText = @"
					CREATE TABLE IF NOT EXISTS Items (
						ItemId INTEGER NOT NULL,
						ItemName TEXT NOT NULL,
						InsDate TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
					);";
					await command.ExecuteNonQueryAsync();
				}


				// Создаем таблицу
				using(var command = connection.CreateCommand())
				{
					command.CommandText = @"
					CREATE TABLE IF NOT EXISTS ItemAttributes (
						ItemId INTEGER NOT NULL,
						AttributeName TEXT NOT NULL,
						AttributeValue TEXT NULL
					);";
					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public async void RemoveItem(Int32 itemId)
		{
			using(var connection = new SqliteConnection(ConnectionString))
			{
				connection.Open();
				using(var command = connection.CreateCommand())
				{
					command.CommandText = @$"DELETE FROM ItemAttributes WHERE ItemId=@itemId";
					command.Parameters.AddWithValue("itemId", itemId);
					await command.ExecuteNonQueryAsync();
				}
				using(var command = connection.CreateCommand())
				{
					command.CommandText = @$"DELETE FROM Items WHERE ItemId=@itemId";
					command.Parameters.AddWithValue("itemId", itemId);
					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public async void AddItem(Int32 itemId, String itemName)
		{
			using(var connection = new SqliteConnection(ConnectionString))
			{
				connection.Open();
				using(var command = connection.CreateCommand())
				{
					command.CommandText = @$"
					INSERT INTO Items (ItemId, ItemName) VALUES (@itemId, @itemName);";
					command.Parameters.AddWithValue("itemId", itemId);
					command.Parameters.AddWithValue("itemName", itemName);
					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public async void AddAttributeItem(Int32 itemId, String attributeName)
		{
			using(var connection = new SqliteConnection(ConnectionString))
			{
				connection.Open();
				using(var command = connection.CreateCommand())
				{
					command.CommandText = @$"INSERT INTO ItemAttributes (ItemId, AttributeName) VALUES (@itemId, @attributeName);";
					command.Parameters.AddWithValue("itemId", itemId);
					command.Parameters.AddWithValue("attributeName", attributeName);

					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public async void AddAttributeItem(Int32 itemId, String attributeName, String attributeValue)
		{
			using(var connection = new SqliteConnection(ConnectionString))
			{
				connection.Open();
				using(var command = connection.CreateCommand())
				{
					command.CommandText = @$"INSERT INTO ItemAttributes (ItemId, AttributeName, AttributeValue) VALUES (@itemId, @attributeName, @attributeValue);";
					command.Parameters.AddWithValue("itemId", itemId);
					command.Parameters.AddWithValue("attributeName", attributeName);
					command.Parameters.AddWithValue("attributeValue", attributeValue);
					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task<DataItem> GetItem(Int32 itemId)
		{
			using(var connection = new SqliteConnection(ConnectionString))
			{
				connection.Open();
				using(var command = connection.CreateCommand())
				{
					command.CommandText = @$"SELECT ItemId, ItemName, InsDate FROM Items WHERE ItemId = @itemId";
					command.Parameters.AddWithValue("itemId", itemId);
					SqliteDataReader reader = await command.ExecuteReaderAsync();
					while(await reader.ReadAsync())
						return new DataItem() { ID = reader.GetInt32(0), Name = reader.GetString(1), InsDate = reader.GetDateTime(2) };
				}
			}
			return null;
		}

		public async IAsyncEnumerable<KeyValuePair<String, String>> GetItemAttributes(Int32 itemId)
		{
			using(var connection = new SqliteConnection(ConnectionString))
			{
				connection.Open();
				using(var command = connection.CreateCommand())
				{
					command.CommandText = @$"SELECT AttributeName, AttributeValue FROM ItemAttributes WHERE ItemId=@itemId";
					command.Parameters.AddWithValue("itemId", itemId);
					SqliteDataReader reader = await command.ExecuteReaderAsync();
					while(await reader.ReadAsync())
					{
						if (reader.IsDBNull(1))
							yield return new KeyValuePair<String, String>(reader.GetString(0), null);
						else
							yield return new KeyValuePair<String, String>(reader.GetString(0), reader.GetString(1));
					}
				}
			}
		}
	}
}
