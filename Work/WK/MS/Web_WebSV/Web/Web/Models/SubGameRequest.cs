using System.Collections.Generic;

namespace Web.Models
{
    public class SubGameRequest
    {
        public int GameTabType { get; set; }

        private int _pageNumber;

        private List<int> _gameIds;

        public List<int> GameIds
        {
            get => _gameIds ?? new List<int>();
            set => _gameIds = value;
        }

        public string GameName { get; set; }

        public int PageNumber
        {
            get => _pageNumber < 1 ? 1 : _pageNumber;
            set => _pageNumber = value;
        }
    }
}