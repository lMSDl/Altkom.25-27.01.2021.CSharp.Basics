﻿using ConsoleApp.Extensions;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApp
{
    public partial class Program
    {
        private static void AutoEdit(Person person)
        {
            person.GetType().GetProperties()
                .Where(x => x.CanWrite && x.CanWrite)
                .Where(x => !x.GetCustomAttributes(true).Any(att => att is JsonIgnoreAttribute))
                .ToList()
                .ForEach(x =>
            {
                Func<string, object> converter;
                var value = x.GetValue(person);
                switch(x.PropertyType.Name)
                {
                    case "String":
                        converter = @string => @string;
                        break;

                    case "DateTime":
                        converter = @string => @string.ToDateTime();
                        value = ((DateTime)value).ToShortDateString(); 
                        break;

                    case "Gender":
                        converter = @string => @string.ToGender();
                        break;

                    default:
                        return;

                }
                x.SetValue(person, ReadPersonData(Properties.Resources.ResourceManager.GetString(x.Name), value?.ToString(), converter));
            });

        }

        /*private static void Edit(Address address)
        {
            address.Street = ReadPersonData(Properties.Resources.Street, address.Street);
            ....
        }*/

        private static void Edit(Person person)
        {
            //Edit(person.Address);
            //person.Address.Street = ReadPersonData(Properties.Resources.Street, person.Address.Street);

            //AutoEdit(person);
            //return;

            /*Console.WriteLine(Properties.Resources.FirstName);
            SendKeys.SendWait(person.FirstName);
            person.FirstName = Console.ReadLine();*/
            person.FirstName = ReadPersonData(Properties.Resources.FirstName, person.FirstName);

            /*Console.WriteLine(Properties.Resources.LastName);
            SendKeys.SendWait(person.LastName);
            person.LastName = Console.ReadLine();*/
            person.LastName = ReadPersonData<string>(Properties.Resources.LastName, person.LastName, x => x.Length < 15 ? x : null);

            /*bool breakCondition;
            do {
                Console.WriteLine(Properties.Resources.Gender);
                SendKeys.SendWait(person.Gender.ToString());
                //person.Gender = (Gender)Enum.Parse(typeof(Gender), Console.ReadLine());
                if (breakCondition = Enum.TryParse<Gender>(Console.ReadLine(), out var gender))
                    person.Gender = gender;
            } while (!breakCondition);*/
            var gender = ReadPersonData(Properties.Resources.Gender, person.Gender.ToString(), x => Enum.TryParse<Gender>(x, out _));
            person.Gender = (Gender)Enum.Parse(typeof(Gender), gender);
            //person.Gender = ReadPersonData<Gender?>(Properties.Resources.Gender, person.Gender.ToString(), x => x.ToGender()).Value;

            /*do
            {
                Console.WriteLine(Properties.Resources.BirthDate);
                SendKeys.SendWait(person.BirthDate.ToShortDateString());
                //person.BirthDate = DateTime.Parse(Console.ReadLine());
                if (breakCondition = DateTime.TryParse(Console.ReadLine(), out var birthDate))
                    person.BirthDate = birthDate;
            } while (!breakCondition);*/
            person.BirthDate = ReadPersonData<DateTime?>(Properties.Resources.BirthDate, person.BirthDate.ToShortDateString(), x => x.ToDateTime()).Value;
        }

        private static string ReadPersonData(string label, string currentValue, Func<string, bool> validator = null /*parametr opcjonalny*/)
        {
            string result;
            //bool breakCondition;
            do
            {
                Console.WriteLine(label);
                SendKeys.SendWait(currentValue);
                result = Console.ReadLine();
                /*if (validator == null)
                    breakCondition = false;
                else
                    breakCondition = validator.Invoke(result);*/
                //breakCondition = validator == null ? false : validator.Invoke(result);
                //    breakCondition = validator?.Invoke(result) ?? false;
                //} while (!breakCondition);
            } while (!validator?.Invoke(result) ?? false);

            return result;
        }

        private static T ReadPersonData<T>(string label, string currentValue, Func<string, T> converter)
        {
            T result;
            do
            {
                Console.WriteLine(label);
                SendKeys.SendWait(currentValue);
                result = converter(Console.ReadLine());
            } while (result?.Equals(default) ?? true);

            return result;
        }
    }
}
