using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BoxOffice.DAL.Interfaces;
using AutoMapper;
using log4net;
using DataContracts = BoxOffice.Api.DataContracts;
using DataLayer = BoxOffice.Model;
using BoxOffice.Api.CustomFilter;

namespace BoxOffice.Api.Controllers
{
    /// <summary>
    /// Master Data API: Provides api for theatre master data.
    /// </summary>
    [RoutePrefix("api/theater")]
    [BoxOfficeAuthorize]
    public class MasterDataController : ApiController
    {
        private ILog Log = LogManager.GetLogger(typeof(MasterDataController));

        private ISeatTemplateRepository _seatTemplateRepository;
        private ISeatLayoutConfig _seatLayoutConfig;
        private IMovieTimingsRepository _movieTimingsRepository;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="seatTemplateRepository">ISeatTemplateRepository</param>
        /// <param name="seatLayoutConfig">ISeatLayoutConfig</param>
        /// <param name="movieTimingsRepository">IMovieTimingsRepository</param>
        public MasterDataController(ISeatTemplateRepository seatTemplateRepository,
            ISeatLayoutConfig seatLayoutConfig,
            IMovieTimingsRepository movieTimingsRepository)
        {
            _seatTemplateRepository = seatTemplateRepository;
            _seatLayoutConfig = seatLayoutConfig;
            _movieTimingsRepository = movieTimingsRepository;
        }

        /// <summary>
        /// Returns list of theatres available.
        /// </summary>
        /// <returns>List of Theatre</returns>
        [Route("details")]
        public IEnumerable<DataContracts.Theatre> GetTheatre()
        {
            try
            {
                var theatre = _seatTemplateRepository.GetTheatres();
                IEnumerable<DataContracts.Theatre> theatreList = new List<DataContracts.Theatre>();
                Mapper.Map<IEnumerable<DataLayer.Theatres>, IEnumerable<DataContracts.Theatre>>(theatre, theatreList);

                return theatreList;
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error occured while getting theatre list: ", ex);
                throw new ServiceException(message: "Internal Server Error occured while processing the request", httpCode: HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Returns available screens list for theatre.
        /// </summary>
        /// <returns>IEnumerable<DataContracts.Screen></returns>
        [Route("screens")]
        public IEnumerable<DataContracts.Screen> GetScreens()
        {
            try
            {
                var screens = _seatLayoutConfig.GetScreens();
                IEnumerable<DataContracts.Screen> screenList = new List<DataContracts.Screen>();
                Mapper.Map(screens, screenList);

                return screenList;
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error occured while getting screens list: ", ex);
                return null;
            }
        }

        /// <summary>
        /// Returns Theatre Screen classes based on screen Id.
        /// </summary>
        /// <param name="screenId">int</param>
        /// <returns>IEnumerable<DataContracts.Screenclasses></returns>
        [Route("screenclasses")]
        public IEnumerable<DataContracts.Screenclasses> GetScreenClasses(int screenId)
        {
            try
            {
                var screenClasses = _seatLayoutConfig.GetScreeClasses(screenId);
                IEnumerable<DataContracts.Screenclasses> screenClassList = new List<DataContracts.Screenclasses>();
                Mapper.Map(screenClasses, screenClassList);

                return screenClassList;
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error occured while getting screen class list: ", ex);
                return null;
            }
        }

        /// <summary>
        /// Returns Movies based on date time.
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <returns>IEnumerable<DataContracts.Movie></returns>
        [Route("movies")]
        public IEnumerable<DataContracts.Movie> GetMovies(DateTime dateTime)
        {
            try
            {
                var movies = _movieTimingsRepository.GetMovies(dateTime);
                IEnumerable<DataContracts.Movie> movieList = new List<DataContracts.Movie>();
                Mapper.Map(movies, movieList);

                return movieList;
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error occured while getting movie list: ", ex);
                return null;
            }
        }

        /// <summary>
        /// Returns movie schedule based on screen Id for mentioned month and year.
        /// </summary>
        /// <param name="screenId">int</param>
        /// <param name="month">int</param>
        /// <param name="year">int</param>
        /// <returns>IEnumerable<DataContracts.MovieTimings></returns>
        [Route("movietimings")]
        public IEnumerable<DataContracts.MovieTimings> GetMovieTimings(int screenId, int month, int year)
        {
            try
            {
                var movieTimings = _movieTimingsRepository.GetScreenMovies(screenId, month, year);
                IEnumerable<DataContracts.MovieTimings> movieTimingList = new List<DataContracts.MovieTimings>();
                Mapper.Map(movieTimings, movieTimingList);

                return movieTimingList;
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error occured while getting movie list: ", ex);
                return null;
            }
        }

        /// <summary>
        /// Returns seat layout based on screen Id and movie schedule timing Id.
        /// </summary>
        /// <param name="screenId">int</param>
        /// <param name="screenClass">int</param>
        /// <param name="moviTiming">int</param>
        /// <returns>IEnumerable<DataContracts.Seat></returns>
        [Route("seatlayout")]
        public IEnumerable<DataContracts.Seat> GetSeatLayout(int vendoId, int screenId, int moviTiming)
        {

            try
            {
                var seats = _seatLayoutConfig.GetVendorSeats(vendoId, screenId, moviTiming);
                IEnumerable<DataContracts.Seat> seatList = new List<DataContracts.Seat>();
                Mapper.Map(seats, seatList);

                return seatList;
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error occured while getting seat list: ", ex);
                return null;
            }
        }
    }
}