using BoxOffice.Model;
using System.Collections.Generic;

namespace BoxOffice.DAL.Interfaces
{
    public interface ISeatTemplateRepository
    {
        int insertSeatTemplate(int templateId, string seats, int screenId, int vendorId, string templateName, string unselectedSeats);

        int insertMgrHolding(List<Seat> seats, string HolderName, int MovieTimingId);

        IEnumerable<Theatres> GetTheatres();
    }
}