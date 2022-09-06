using GameRegistrationNETApp.Enums;

namespace GameRegistrationNETApp
{
    public class Game : BaseEntity
    {
        private string _title = string.Empty;
		private string _description = string.Empty;
		private int _year;
        private Genre _genre;
        private bool _deleted = false;

        public int Id { get => _id; set => _id = value; }
        public string Title { get => _title; set => _title = value; }
        public string Description { get => _description; set => _description = value; }
        public int Year { get => _year; set => _year = value; }
        public Genre Genre { get => _genre; set => _genre = value; }
        public bool Deleted { get => _deleted; set => _deleted = value; }

        public override string ToString()
        {
            string result = "";
            result += "Titulo: " + _title + Environment.NewLine;
            result += "Descrição: " + _description + Environment.NewLine;
            result += "Gênero: " + _genre + Environment.NewLine;
            result += "Ano de Início: " + _year;
            result += "Excluído: " + (_deleted ? "Sim" : "Não");
			return result;
        }
    }
}