using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JSONHomework
{
	public static class Person
	{
        private static string jsonFile = @"D:\Work.json";
        private static int id;
        private static string firstName;
        private static string lastName;
        private static decimal salaryPerHour;
        private static string json = File.ReadAllText(jsonFile);
        private static JArray experiencesArrary = JArray.Parse(json);

        private static void Add(string[] arguments)
        {
            var newId = 0;
            foreach (var item in experiencesArrary)
            {
                if (Convert.ToInt32(item["id"]) > newId)
                    newId = Convert.ToInt32(item["id"]);
            }
            id = newId + 1;

            if (arguments[1].Split(':')[0].ToLower() == "firstname")
                firstName = arguments[1].Split(':')[1];
            else
                throw new Exception("Invalid argument");

            if (arguments[2].Split(':')[0].ToLower() == "lastname")
                lastName = arguments[2].Split(':')[1];
            else
                throw new Exception("Invalid argument");

            if (arguments[3].Split(':')[0].ToLower() == "salary")
                salaryPerHour = Convert.ToDecimal(arguments[3].Split(':')[1]);
            else
                throw new Exception("Invalid argument");

            var newRecord = "{" +
                "\n\"id\":" + id + "," +
                "\n\"firstname\":\"" + firstName + "\"," +
                "\n\"lastname\":\"" + lastName + "\"," +
                "\n\"salary\":" + salaryPerHour + "" +
                "\n}";
            try
            {
                var newPerson = JObject.Parse(newRecord);

                experiencesArrary.Add(newPerson);

                string newJsonResult = JsonConvert.SerializeObject(experiencesArrary, Formatting.Indented);
                File.WriteAllText(jsonFile, newJsonResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add Error : " + ex.Message.ToString());
            }
        }

        private static void Update(string[] arguments)
		{
            int id = Convert.ToInt32(arguments[1].Split(':')[1]);

            foreach (var company in experiencesArrary)
            {
                if (id != Convert.ToInt32(company["id"]))
                    throw new InvalidOperationException();
            }

            for (int i = 2; i < arguments.Count(); i++)
			{
                if (id > 0)
                {
                    foreach (var company in experiencesArrary.Where(obj => obj["id"].Value<int>() == id))
                    {
                        company[arguments[i].Split(':')[0].ToLower()] = !string.IsNullOrEmpty(arguments[i].Split(':')[1]) ? arguments[i].Split(':')[1] : "";
                    }
                
                    string output = JsonConvert.SerializeObject(experiencesArrary, Formatting.Indented);
                    File.WriteAllText(jsonFile, output);
                }
                else
                {
                    throw new InvalidDataException();
                }
			}
        }

        private static void Delete(string arguments)
        {
            int newId = Convert.ToInt32(arguments.Split(':')[1]);
            try
            {
                if (newId > 0)
                {
                    var companyToDeleted = experiencesArrary.FirstOrDefault(obj => obj["id"].Value<int>() == newId);

                    experiencesArrary.Remove(companyToDeleted);

                    string output = JsonConvert.SerializeObject(experiencesArrary, Formatting.Indented);
                    File.WriteAllText(jsonFile, output);
                }
                else
                {
                    throw new InvalidDataException();
                }
            }
            catch (Exception)
            {

                throw new OperationCanceledException();
            }
        }
        
        private static void Get(string arguments)
        {
            int newId = Convert.ToInt32(arguments.Split(':')[1]);
            try
            {
                if (experiencesArrary != null)
                {
                    var companyToSelect = experiencesArrary.FirstOrDefault(obj => obj["id"].Value<int>() == newId);

					Console.WriteLine(experiencesArrary[experiencesArrary.IndexOf(companyToSelect)]);
                }
                else
                {
                    throw new Exception("The list is empty");
                }
            }
            catch (Exception)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        private static void GetAll()
        {
			try
			{
				if (experiencesArrary != null)
				{
					foreach (var item in experiencesArrary)
					{
						Console.WriteLine("{" +
										"\n\"id\":" + item["id"] + "," +
										"\n\"firstname\":\"" + item["firstname"] + "\"," +
										"\n\"lastname\":\"" + item["lastname"] + "\"," +
										"\n\"salary\":" + item["salary"] + "" +
										"\n}");
					}
				}
				else
				{
					throw new NullReferenceException();
				}
			}
			catch (Exception)
			{
				throw new Exception();
			}
		}

        public static void GetResult(string[] request)
		{
            switch (request[0])
			{
                case "-add":
                    Add(request);
                    break;
                case "-update":
                    Update(request);
                    break;
                case "-get":
                    Get(request[1]);
                    break;
                case "-delete":
                    Delete(request[1]);
                    break;
                case "-getall":
                    GetAll();
                    break;
			}
		}
    }
}
