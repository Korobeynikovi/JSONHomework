using JSONHomework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.IO;

namespace TestHomeWork
{
	public class AddingTest
	{
		[Test]
		public void Adding_Test()
		{
			string jsonFile = @"D:\Work.json";
			string json = File.ReadAllText(jsonFile);
			JArray experiencesArrary = JArray.Parse(json);
			
			string[] m = { "-add", "FirstName:John", "LastName:Wick", "Salary:120.30" };
			Person.GetResult(m);
			
			var newExAr = experiencesArrary;
			
			var newRecord = "{" +
				"\n\"id\":\"4\"," +
				"\n\"firstname\":\"John\"," +
				"\n\"lastname\":\"Wick\"," +
				"\n\"salary\":\"120.30\"" +
				"\n}";

			var newPerson = JObject.Parse(newRecord);

			newExAr.Add(newPerson);

			string newJsonResult = JsonConvert.SerializeObject(experiencesArrary, Formatting.Indented);
			File.WriteAllText(jsonFile, newJsonResult);

			Assert.AreEqual(experiencesArrary, newExAr);
		}
	}
}