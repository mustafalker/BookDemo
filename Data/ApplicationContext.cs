using bookDemo.Models;

namespace bookDemo.Data
{
    public static class ApplicationContext //
    {
        public static List<Book> Books { get; set; }
        static ApplicationContext() // static nesneler için bu şekilde girmemiz lazım 
        {
            Books = new List<Book> () // bellek üzerinde static bir nesne bir değişiklikte herkes bundan etkilenmiş olur 
            {
                new Book(){ Id = 1 , Title = "Karagöz ve Hacivat" , Price = 75},
                new Book(){ Id = 2 , Title = "Mesnevi" , Price = 150},
                new Book(){ Id = 3, Title = "Dede Korkut", Price = 75} //buraya data olarak girdiğimiz verilerin bize api olarak dönmmesini sağlayabiliyoruz 
            }; 
        }
    }
}
