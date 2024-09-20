using CM.Models.Models;
using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.Data.Service.IService
{
	public interface IContactInfoService
	{
		public ContactInfo? ConvertFromCsv(IFormFile file)
		{
			ContactInfo? info = null;

			using (var reader = new StreamReader(file.OpenReadStream()))
			{
				var config = new CsvConfiguration(CultureInfo.InvariantCulture)
				{
					Delimiter = ",", // Кома як розділювач
					HasHeaderRecord = true // Якщо є заголовки у файлі
				};

				using (var csv = new CsvReader(reader, config))
				{
					// Читаємо заголовок
					csv.Read();
					// Читаємо заголовки
					csv.ReadHeader();
					// Якщо будь-яке поле відсутнє в заголовку, повертаємо null
					var headerNames = csv.HeaderRecord;
					if (!headerNames.Contains("Name") ||
						!headerNames.Contains("DateOfBirth") ||
						!headerNames.Contains("Married") ||
						!headerNames.Contains("Phone") ||
						!headerNames.Contains("Salary"))
					{
						return null; 
					}

					// Читаємо перший запис
					if (csv.Read())
					{
						// Перевірка на наявність всіх необхідних полів
						var name = csv.GetField("Name");
						var dateOfBirth = csv.GetField("DateOfBirth");
						var married = csv.GetField("Married");
						var phone = csv.GetField("Phone");
						var salary = csv.GetField("Salary");

						// Якщо будь-яке поле відсутнє або порожнє, то повертаємо null
						if (string.IsNullOrWhiteSpace(name) ||
							string.IsNullOrWhiteSpace(dateOfBirth) ||
							string.IsNullOrWhiteSpace(married) ||
							string.IsNullOrWhiteSpace(phone) ||
							string.IsNullOrWhiteSpace(salary))
						{
							return null;
						}

						// Перевірка дати народження
						DateOnly DOB; // Date Of Birth
						if (!DateOnly.TryParseExact(dateOfBirth, "dd.MM.yyyy",
							CultureInfo.InvariantCulture,
							DateTimeStyles.None,
							out DOB))
						{
							return null; // Невірний формат дати
						}

						// Перевірка поля Married
						bool marriedConverted;
						if (married == "1" || married
							.Equals("Yes", StringComparison.OrdinalIgnoreCase))
						{
							marriedConverted = true;
						}
						else if (married == "0" ||
							married.Equals("No", StringComparison.OrdinalIgnoreCase))
						{
							marriedConverted = false;
						}
						else
						{
							return null; // Некоректне значення для Married
						}

						// Перевірка зарплати
						if (!double.TryParse(salary.Replace(",", "."),
							NumberStyles.Any,
							CultureInfo.InvariantCulture,
							out double salaryConverted))
						{
							return null; // Некоректне значення для зарплати
						}

						// Якщо всі перевірки пройшли успішно, створюємо об'єкт ContactInfo
						info = new ContactInfo
						{
							Name = name,
							DateOfBirth = DOB,
							Married = marriedConverted,
							Phone = phone,
							Salary = salaryConverted
						};
					}
				}
			}

			return info;
		}

	}
}
