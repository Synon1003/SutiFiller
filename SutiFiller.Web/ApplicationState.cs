namespace SutiFiller.Web
{
    public class ApplicationState
    {
        private long _userCount;

        public long UserCount
        {
            get => Interlocked.Read(ref _userCount);
            set => Interlocked.Exchange(ref _userCount, value);
        }
    }
}
