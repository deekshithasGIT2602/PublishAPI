

using System.ComponentModel.DataAnnotations;


namespace PublishAPI.Models
{
    
    public class VoterIDRequest
    {
        [Required]
        public string voterid { get; set; }


    }
}
