using Elmah;

namespace NordicArenaTournament.ViewModels
{
    public class LoggedElmahErrorViewModel : _LayoutViewModel
    {
        public ErrorLogEntry Error { get; set; }
        public LoggedElmahErrorViewModel() { }
    }
}