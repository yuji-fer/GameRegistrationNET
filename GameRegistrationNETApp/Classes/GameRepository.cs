using GameRegistrationNETApp.Interfaces;

namespace GameRegistrationNETApp
{
    public class GameRepository : IBaseRepository<Game>
    {
        private readonly List<Game> _games;
        private const string CONST_GAME_NOT_FOUND = "Game not found.";
        private const string CONST_GAME_CANNOT_BE_NULL = "The Game cannot be null.";
        private const string CONST_GAME_DATA_INVALID = "The Game data is invalid.";

        public GameRepository(List<Game> games)
        {
            _games = games;
        }

        public GameRepository() : this(new List<Game>())
        {
        }

        public void Delete(int id)
        {
            if (!GameExist(id))
                throw new ArgumentOutOfRangeException(null, CONST_GAME_NOT_FOUND);

            _games[id].Deleted = true;
        }

        public Game GetById(int id)
        {
            if (!GameExist(id))
                throw new ArgumentOutOfRangeException(null, CONST_GAME_NOT_FOUND);
            
            return _games[id];
        }

        public void Insert(Game entity)
        {
            if (entity == null)
                throw new ArgumentNullException(null, CONST_GAME_CANNOT_BE_NULL);
                
            if (!GameValid(entity))
                throw new ArgumentException(CONST_GAME_DATA_INVALID);
            
            entity.Id = _games.Count;
            _games.Add(entity);
        }

        public List<Game> GetAll() => _games;

        public void Update(int id, Game entity)
        {
            if (!GameExist(id))
                throw new ArgumentOutOfRangeException(null, CONST_GAME_NOT_FOUND);
            
            if (entity == null)
                throw new ArgumentNullException(null, CONST_GAME_CANNOT_BE_NULL);
            
            if (!GameValid(entity))
                throw new ArgumentException(CONST_GAME_DATA_INVALID);
            
            entity.Id = id;
            _games[id] = entity;
        }

        private bool GameValid(Game game)
        {
            if (
                string.IsNullOrEmpty(game.Title) || 
                string.IsNullOrEmpty(game.Description) || 
                game.Year < 0
            )
                return false;

            return true;
        }

        private bool GameExist(int id)
        {
            return id <= (_games.Count - 1);
        }
    }
}