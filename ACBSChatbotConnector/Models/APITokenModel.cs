namespace UserService.Models
{
    public class APITokenModel
    {
        [CustomRequired(105)]

        public string access_token {get;set;}
        [CustomRequired(106)]

        public string refresh_token {get;set;}
    }
}
