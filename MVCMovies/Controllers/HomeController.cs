using MVCMovies.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MVCMovies.Controllers
{
    public class HomeController : Controller
    {
        //create a database connection object, using the
        //model created with ADO.NET
        private MoviesEntities db = new MoviesEntities();

        public ActionResult Index(string searchString, string movieGenre)
        {
            //creating the SelectList for the Genre dropdown list ----------------------

            //declare a list of strings (add using System.Collections.Generic above)
            List<string> genreList = new List<string>();
            //get the genres from the database in ascending alphabetical order
            var genreQuery = from g in db.Movies
                         orderby g.Genre
                         select g.Genre;
            //add the unique values of genres from the db query to the list
            genreList.AddRange(genreQuery.Distinct());
            //convert the list of strings to a SelectList (the data type that
            //is needed for the list items in dropdown lists) and put it into
            //the ViewBag, so the view can find it
            ViewBag.movieGenre = new SelectList(genreList);
            //----------------------------------------------------------------------------

            //LINQ query to get all the records from the db.  Since there is probably more than
            //one record, the data will be some kind of collection.  Using var means that you don't
            //have to know exactly (though the compiler definitely knows!).  However, the view that 
            //you pass it into will need IEnumerable in its model statement to handle the collection
            var movies = from m in db.Movies
                         select m;

            //filtering the movies from the db by genre
            if (!String.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieGenre);
            }

            //searching the movies from the db by title
            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(x => x.Title.Contains(searchString));
            }

            //passing the data to the view
            return View(movies);
        }

        //display the create view with no data
        public ActionResult Create()
        {
            return View();
        }

        //HttpPost tells MVC to communicate with the server
        //using POST (not GET) when executing this action
        //method - this means that data on the server can be 
        //changed
        [HttpPost]
        //This checks that the view that the action method calls is
        //the right one - it prevents CSRF (cross-site request forgery)
        //attacks
        [ValidateAntiForgeryToken]
        //a Movie object with the data from the Create view
        //is passed into this action method
        //binding is used to ensure that only content for certain fields
        //is accepted
        public ActionResult Create([Bind(Include = "Id,Title,ReleaseDate,Genre,Price,ImageUrl")]Movie movie)
        {
            //if the ImageUrl field is empty, give it a default value to stop the Url.Content
            //helper crashing the application
            if (movie.ImageUrl == null)
            {
                movie.ImageUrl = "https://pbs.twimg.com/profile_images/836629578554748928/DHbaSYYv_400x400.jpg";
            }
            if (ModelState.IsValid)
            {
                //the movie record is lined up to be
                //added to the database
                db.Movies.Add(movie);
                //the record to be added is actually sent to the server
                //and added to the database
                db.SaveChanges();
                //the user is redirected to the Index View
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        //this method gets a record id from the Html.ActionLink
        //that called it
        public ActionResult Edit(int? id)
        {
            //if a null id is passed in, display an HTML error page
            //add using System.Net to get rid of the error on HttpStatusCode
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //get the record from the database using its id
            Movie movie = db.Movies.Find(id);
            //if an invalid id was passed in, and the record doesn't
            //exist in the database, display an HTML error page
            if (movie == null)
            {
                return HttpNotFound();
            }
            //pass the data to the Details view to be displayed
            return View(movie);
        }

        //HttpPost tells MVC to communicate with the server
        //using POST (not GET) when executing this action
        //method - this means that data on the server can be 
        //changed
        [HttpPost]
        //This checks that the view that the action method calls is
        //the right one - it prevents CSRF (cross-site request forgery)
        //attacks
        [ValidateAntiForgeryToken]
        //a Movie object with the data from the Edit view
        //is passed into this action method
        //binding is used to ensure that only content for certain fields
        //is accepted
        public ActionResult Edit([Bind(Include = "Id,Title,ReleaseDate,Genre,Price,ImageUrl")]Movie movie)
        {
            //if the ImageUrl field is empty, give it a default value to stop the Url.Content
            //helper crashing the application
            if (movie.ImageUrl == null)
            {
                movie.ImageUrl = "https://pbs.twimg.com/profile_images/836629578554748928/DHbaSYYv_400x400.jpg";
            }
            if (ModelState.IsValid)
            {
                //the edited record is tagged for changing at
                //the next database update
                db.Entry(movie).State = EntityState.Modified;
                //database is updated
                db.SaveChanges();
                //user is returned to the Index view
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        //this method gets a record id from the Html.ActionLink
        //that called it
        public ActionResult Details(int? id)
        {
            //if a null id is passed in, display an HTML error page
            //add using System.Net to get rid of the error on HttpStatusCode
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //get the record from the database using its id
            Movie movie = db.Movies.Find(id);
            //if an invalid id was passed in, and the record doesn't
            //exist in the database, display an HTML error page
            if (movie == null)
            {
                return HttpNotFound();
            }
            //pass the data to the Details view to be displayed
            return View(movie);
        }

        //this method gets a record id from the Html.ActionLink
        //that called it
        public ActionResult Delete(int? id)
        {
            //if a null id is passed in, display an HTML error page
            //add using System.Net to get rid of the error on HttpStatusCode
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //get the record from the database using its id
            Movie movie = db.Movies.Find(id);
            //if an invalid id was passed in, and the record doesn't
            //exist in the database, display an HTML error page
            if (movie == null)
            {
                return HttpNotFound();
            }
            //pass the data to the Details view to be displayed
            return View(movie);
        }

        //HttpPost tells MVC to communicate with the server
        //using POST (not GET) when executing this action
        //method - this means that data on the server can be 
        //changed
        //ActionName("Delete") tells MVC to treat this action
        //method as though it is called Delete (it couldn't be
        //called Delete when it was first writtenbecause there 
        //was another Delete action method with the same method
        //signature, which C# wouldn't accept.  The signature for
        //the other Delete action method was changed later, so the
        //ActionName annotation isn't needed any more, but has been
        //left in as an example)
        [HttpPost, ActionName("Delete")]
        //This checks that the view that the action method calls is
        //the right one - it prevents CSRF (cross-site request forgery)
        //attacks
        [ValidateAntiForgeryToken]
        //the record id is passed in from the form in the Delete
        //view
        public ActionResult DeleteConfirmed(int id)
        {
            //get the record from the database using its id
            Movie movie = db.Movies.Find(id);
            //mark the record for deletion when the database
            //is next updated
            db.Movies.Remove(movie);
            //update the database
            db.SaveChanges();
            //return the user to the Index view
            return RedirectToAction("Index");
        }

        //disposes of unneeded resources to free up memory
        //called automatically by MVC when a resource is no longer needed
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
        //these action methods are not needed, although you can use
        //them if you want to
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}