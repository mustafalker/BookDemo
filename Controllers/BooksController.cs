using bookDemo.Data;
using bookDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace bookDemo.Controllers
{
    [Route("api/books")]
    [ApiController] //Behaivor kazandırıyor . 
    public class BooksController : ControllerBase
    {
        #region HttpGet
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = ApplicationContext.Books.ToList();
            return Ok(books);
        }
        #endregion

        #region HttpGet
        [HttpGet("{id:int}")] //burayada get dersek api bunları ayırt edemez ise hata ile karşılaşırız 
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id) //[FromRoute(Name="id")] bu kısım olmasada kodumuz çalışır ancak bu bize verini geldiği yeri daha sağlıklı olarak bilmemizi sağlıyor
        {
            var book = ApplicationContext
                .Books
                .Where(b => b.Id.Equals(id))
                .SingleOrDefault(); //LINQ dile entegre sorgu ifadesi . Default değeri nulldır 
            if (book is null)
                return NotFound(); //404 

            return Ok(book);
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book) //Requestin bodyden geleceğini söyledik
        {
            try//deneme
            {
                if (book is null)
                    return BadRequest(); // 400 döner 

                ApplicationContext.Books.Add(book); //gelen listeryi eklemiş olacağız .
                return StatusCode(201, book);
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }
        #endregion

        #region HttpGet
        [HttpPut("{id:int}")] // tip güvenliği açısından int değerini girmek mantıklı
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id,
            [FromBody] Book book)
        {
            //check book var mı  ?
            var entity = ApplicationContext
                .Books
                .Find(b => b.Id.Equals(id));

            if (entity is null)
            {
                return NotFound(); //404
            }

            // check id 
            if (id != book.Id)
            {
                return BadRequest(); //400 
            }

            ApplicationContext.Books.Remove(entity);
            book.Id = entity.Id;
            ApplicationContext.Books.Add(book);
            return Ok(book);
        }
        #endregion

        #region HttpDelete
        [HttpDelete]
        public IActionResult DeleteAllBooks()
        {
            ApplicationContext.Books.Clear();
            return NoContent(); //204 
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            var entity = ApplicationContext.Books
                .Find(b => b.Id.Equals(id));

            if (entity is null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = $"Book With id:{id} could not found."
                });//404
            ApplicationContext.Books.Remove(entity);
            return NoContent();
        }
        #endregion

        #region HttpPatch
        [HttpPatch("{id:int}")]
        public IActionResult PurtiallyUpdateOneBook([FromRoute(Name = "id")] int id,
            [FromBody] JsonPatchDocument<Book> bookPatch)//bu from route nerden geliyor tam çözemedim 
        {
            //check
            var entity = ApplicationContext.Books.Find(b => b.Id.Equals(id));
            if (entity is null)
            {
                return NotFound(); //404
            }

            bookPatch.ApplyTo(entity);
            return NoContent(); //204 
        } 
        #endregion
            
    }   
}
