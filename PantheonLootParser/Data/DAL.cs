using Emgu.CV.CvEnum;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PantheonLootParser
{
	internal class DAL
	{
		const string ConnectionString = "Data Source=descriptions.db";
		public DAL() {
		}

		public async Task EnsureDBCreated()
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
						InsDate TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
						PRIMARY KEY (ItemId)
					);";
					await command.ExecuteNonQueryAsync();
				}

				using(var command = connection.CreateCommand())
				{
					command.CommandText = @"
					CREATE TABLE IF NOT EXISTS ItemAttributes (
						ItemId INTEGER NOT NULL,
						AttributeName TEXT NOT NULL,
						AttributeValue TEXT NULL,
						Sort INTEGER NOT NULL,
						PRIMARY KEY (ItemId, AttributeName, AttributeValue)
					);";
					await command.ExecuteNonQueryAsync();
				}

				using(var command = connection.CreateCommand())
				{
					command.CommandText = @"
					CREATE TABLE IF NOT EXISTS UserSettings (
						SettingId INTEGER NOT NULL,
						Value TEXT NOT NULL,
						PRIMARY KEY (SettingId)
					);";
					await command.ExecuteNonQueryAsync();
				}

				Boolean isUserSettingsFilled = false;
				using(var command = connection.CreateCommand())
				{
					command.CommandText = @"SELECT Count(1) FROM UserSettings";
					isUserSettingsFilled = (Int64?)await command.ExecuteScalarAsync() > 0;
				}

				if(!isUserSettingsFilled)
				{
					using(var command = connection.CreateCommand())
					{
						command.CommandText = @$"
						BEGIN TRANSACTION;
						INSERT INTO UserSettings VALUES ({(Int32)UserSettingsEnum.ChatPrefix}, '/g \n');
						INSERT INTO UserSettings VALUES ({(Int32)UserSettingsEnum.ResultAction}, '1');
						INSERT INTO UserSettings VALUES ({(Int32)UserSettingsEnum.AttributeSplitter}, '\n');
						INSERT INTO UserSettings VALUES ({(Int32)UserSettingsEnum.ItemSplitter}, '\n\n');
						COMMIT;";
						await command.ExecuteNonQueryAsync();
					}
				}

				using(var command = connection.CreateCommand())
				{
					command.CommandText = @"
					CREATE TABLE IF NOT EXISTS SkipItems (
						SkipText TEXT NOT NULL,
						IsChecked BOOLEAN NOT NULL DEFAULT TRUE,
						PRIMARY KEY (SkipText)
					);";
					await command.ExecuteNonQueryAsync();
				}

				Boolean isSkipItemsFilled = false;
				using(var command = connection.CreateCommand())
				{
					command.CommandText = @"SELECT Count(1) FROM SkipItems";
					isSkipItemsFilled = (Int64?)await command.ExecuteScalarAsync() > 0;
				}

				if(!isSkipItemsFilled)
				{
					using(var command = connection.CreateCommand())
					{
						command.CommandText = @"
						BEGIN TRANSACTION;
						INSERT INTO SkipItems VALUES ('General', TRUE);
						INSERT INTO SkipItems VALUES ('Resource', TRUE);
						INSERT INTO SkipItems VALUES ('Reagent', TRUE);
						INSERT INTO SkipItems VALUES ('Component', TRUE);
						INSERT INTO SkipItems VALUES ('Food', TRUE);
						INSERT INTO SkipItems VALUES ('Ingredient', TRUE);
						INSERT INTO SkipItems VALUES ('Material', TRUE);
						INSERT INTO SkipItems VALUES ('Clickie', TRUE);
						COMMIT;";
						await command.ExecuteNonQueryAsync();
					}
				}

				using(var command = connection.CreateCommand())
				{
					command.CommandText = @"
					CREATE TABLE IF NOT EXISTS Replacements (
						SearchText TEXT NOT NULL,
						ReplacementText TEXT NOT NULL,
						PRIMARY KEY (SearchText)
					);";
					await command.ExecuteNonQueryAsync();
				}

				Boolean isReplacementsFilled = false;
				using(var command = connection.CreateCommand())
				{
					command.CommandText = @"SELECT Count(1) FROM Replacements";
					isReplacementsFilled = (Int64?)await command.ExecuteScalarAsync() > 0;
				}

				if(!isReplacementsFilled)
				{
					using(var command = connection.CreateCommand())
					{
						command.CommandText = @"INSERT INTO Replacements VALUES ('&centerdot;', '·')";
						command.ExecuteNonQuery();
					}
				}
			}
		}

		public async Task RemoveItem(Int32 itemId)
		{
			using(var connection = new SqliteConnection(ConnectionString))
			{
				connection.Open();
				using(var command = connection.CreateCommand())
				{
					command.CommandText =
						@$"	BEGIN TRANSACTION;
							DELETE FROM ItemAttributes WHERE ItemId=@itemId;
							DELETE FROM Items WHERE ItemId = @itemId;
							COMMIT;";
					command.Parameters.AddWithValue("itemId", itemId);
					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task AddItem(Int32 itemId, String itemName)
		{
			using(var connection = new SqliteConnection(ConnectionString))
			{
				connection.Open();
				using(var command = connection.CreateCommand())
				{
					command.CommandText = @$"INSERT INTO Items (ItemId, ItemName) VALUES (@itemId, @itemName);";
					command.Parameters.AddWithValue("itemId", itemId);
					command.Parameters.AddWithValue("itemName", itemName);
					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task AddAttributeItem(Int32 itemId, String attributeName, Int32 sortIndex)
		{
			using(var connection = new SqliteConnection(ConnectionString))
			{
				connection.Open();
				using(var command = connection.CreateCommand())
				{
					command.CommandText = @$"INSERT INTO ItemAttributes (ItemId, AttributeName, Sort) VALUES (@itemId, @attributeName, @sortIndex);";
					command.Parameters.AddWithValue("itemId", itemId);
					command.Parameters.AddWithValue("attributeName", attributeName);
					command.Parameters.AddWithValue("sortIndex", sortIndex);
					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task AddAttributeItem(Int32 itemId, String attributeName, String attributeValue, Int32 sortIndex)
		{
			using(var connection = new SqliteConnection(ConnectionString))
			{
				connection.Open();
				using(var command = connection.CreateCommand())
				{
					command.CommandText = @$"INSERT INTO ItemAttributes (ItemId, AttributeName, AttributeValue, Sort) VALUES (@itemId, @attributeName, @attributeValue, @sortIndex);";
					command.Parameters.AddWithValue("itemId", itemId);
					command.Parameters.AddWithValue("attributeName", attributeName);
					command.Parameters.AddWithValue("attributeValue", attributeValue);
					command.Parameters.AddWithValue("sortIndex", sortIndex);
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
					command.CommandText = @$"SELECT AttributeName, AttributeValue FROM ItemAttributes WHERE ItemId=@itemId ORDER BY Sort";
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

		public async IAsyncEnumerable<KeyValuePair<String, String>> GetReplacements()
		{
			using(var connection = new SqliteConnection(ConnectionString))
			{
				connection.Open();
				using(var command = connection.CreateCommand())
				{
					command.CommandText = @$"SELECT SearchText, ReplacementText IsChecked FROM Replacements";
					SqliteDataReader reader = await command.ExecuteReaderAsync();
					while(await reader.ReadAsync())
						yield return new KeyValuePair<String, String>(reader.GetString(0), reader.GetString(1));
				}
			}
		}

		public async Task AddReplacement(String searchText, String replacementText)
		{
			using(var connection = new SqliteConnection(ConnectionString))
			{
				connection.Open();
				using(var command = connection.CreateCommand())
				{
					command.CommandText = @"INSERT INTO Replacements (SearchText, ReplacementText) VALUES (@searchText, @replacementText)";
					command.Parameters.AddWithValue("searchText", searchText);
					command.Parameters.AddWithValue("replacementText", replacementText);
					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task RemoveReplacement(String searchText)
		{
			using(var connection = new SqliteConnection(ConnectionString))
			{
				connection.Open();
				using(var command = connection.CreateCommand())
				{
					command.CommandText = @"DELETE FROM Replacements WHERE SearchText = @searchText";
					command.Parameters.AddWithValue("searchText", searchText);
					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public async IAsyncEnumerable<KeyValuePair<String, Boolean>> GetSkipItems()
		{
			using(var connection = new SqliteConnection(ConnectionString))
			{
				connection.Open();
				using(var command = connection.CreateCommand())
				{
					command.CommandText = @$"SELECT SkipText, IsChecked FROM SkipItems";
					SqliteDataReader reader = await command.ExecuteReaderAsync();
					while(await reader.ReadAsync())
						yield return new KeyValuePair<String, Boolean>(reader.GetString(0), reader.GetBoolean(1));
				}
			}
		}

		public async Task AddSkipItem(String skipText)
		{
			using(var connection = new SqliteConnection(ConnectionString))
			{
				connection.Open();
				using(var command = connection.CreateCommand())
				{
					command.CommandText = @"INSERT INTO SkipItems VALUES (@skipText, TRUE);";
					command.Parameters.AddWithValue("skipText", skipText);
					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task UpdateSkipItem(String skipText, Boolean isChecked)
		{
			using(var connection = new SqliteConnection(ConnectionString))
			{
				connection.Open();
				using(var command = connection.CreateCommand())
				{
					command.CommandText = @"UPDATE SkipItems SET IsChecked = @isChecked WHERE SkipText = @skipText";
					command.Parameters.AddWithValue("skipText", skipText);
					command.Parameters.AddWithValue("isChecked", isChecked);
					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task RemoveSkipItem(String skipText)
		{
			using(var connection = new SqliteConnection(ConnectionString))
			{
				connection.Open();
				using(var command = connection.CreateCommand())
				{
					command.CommandText = @"DELETE FROM SkipItems WHERE SkipText = @skipText";
					command.Parameters.AddWithValue("skipText", skipText);
					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public String? GetSettings(UserSettingsEnum settingId)
		{
			using(var connection = new SqliteConnection(ConnectionString))
			{
				connection.Open();
				using(var command = connection.CreateCommand())
				{
					command.CommandText = @$"SELECT Value FROM UserSettings WHERE SettingId=@settingId";
					command.Parameters.AddWithValue("settingId", (Int32)settingId);
					return (String?)command.ExecuteScalar();
				}
			}
		}

		public async Task UpdateSettings(UserSettingsEnum settingId, String value)
		{
			using(var connection = new SqliteConnection(ConnectionString))
			{
				connection.Open();
				using(var command = connection.CreateCommand())
				{
					command.CommandText = @$"INSERT OR REPLACE INTO UserSettings (SettingId, Value) VALUES (@settingId, @value);";
					command.Parameters.AddWithValue("settingId", (Int32)settingId);
					command.Parameters.AddWithValue("value", value);
					await command.ExecuteNonQueryAsync();
				}
			}
		}
	}
}
