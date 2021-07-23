using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
namespace Reflaction
{
    class Program
    {
        static void Main(string[] args)
        {
            //User user = new("Akhmadjon", 10);
            //Type type = typeof(User);
            //var propo = type.GetProperties().Where(p => Attribute.IsDefined(p, typeof(AgeValidationAttribute)));
            Document document = new() { FirstName = "Akhmadjon", LastName = "Jurayev", FullName = "Akhmadjon Jurayev", Name = "Kalinus", EmailAddress = "kalinus@gmail.com" };
            TakeOut(document);
            
            TakeOutValueWithFunction(document, p => p.GetType().GetProperty("FirstName"));

            TakeOutValueWithExpression(document, p => new { p.LastName});
            Console.ReadKey();
        }
        static void TakeOut<TAxmad>(TAxmad document)
        {
            var att = typeof(TAxmad).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Translatable)));
            foreach(var at in att)
            {
                var member = document.GetType().GetProperty(at.Name).GetValue(document, null);
                Console.WriteLine(member);
            }
        }
        static void TakeOutValueWithFunction<TAxmad>(TAxmad document,Func<TAxmad,PropertyInfo> select)
        {
            var prop = select.Invoke(document).Name;
            var value = typeof(TAxmad).GetProperty(prop).GetValue(document, null);
            Console.WriteLine(value);
        }
        static void TakeOutValueWithExpression<TAxmad>(TAxmad document, Expression<Func<TAxmad, object>> expression)
        {
            var propName = ((PropertyInfo)((NewExpression)expression.Body).Members[0]).Name;
            var value = typeof(TAxmad).GetProperty(propName).GetValue(document, null);
            Console.WriteLine(value);
        }
    }
    public class User
    { 
        public string Name { get; set; }
        [AgeValidation(23)]
        public int Age { get; set; }
        public User(string n, int a)
        {
            Name = n;
            Age = a;
        }
        public void Display()
        {
            Console.WriteLine($"Имя: {Name}  Возраст: {Age}");
        }
        public int Payment(int hours, int perhour)
        {
            return hours * perhour;
        }
    }
    public class AgeValidationAttribute : Attribute
    {
        public int Age { get; set; }

        public AgeValidationAttribute()
        { }

        public AgeValidationAttribute(int age)
        {
            Age = age;
        }
    }
    public class Document
    {
        [Translatable]
        public string FirstName { get; set; }
        [Translatable]
        public string LastName { get; set; }
        public string FullName { get; set; }
        [Translatable]
        public string Name { get; set; }
        public string EmailAddress { get; set; }
    }
    public class Translatable : Attribute
    {
        public Translatable()
        {

        }
    }
}
