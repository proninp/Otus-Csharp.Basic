namespace MTLServiceBot.Bot.Commands
{
    public class Command
    {
        private readonly string _name;
        private readonly string _description;
        private readonly bool _isRequireAuthentication;
        private readonly ICommand _command;
        public string Name { get => _name; }
        public string Description { get => _description; }
        public bool IsRequireAuthentication { get => _isRequireAuthentication; }
        public ICommand TgCommand { get => _command; }
        public Command(string name, string description, bool isRequireAuthentication, ICommand command)
        {
            _name = name;
            _description = description;
            _isRequireAuthentication = isRequireAuthentication;
            _command = command;
        }
    }
}
