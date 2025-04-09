using Amazon.DynamoDBv2.DataModel;

namespace LibraryManager.Core.Entities.NoSQL
{
    [DynamoDBTable("BookReviews")]
    public class BookReview
    {
        [DynamoDBHashKey("PK")]
        public string PK { get; set; }

        [DynamoDBProperty]
        public string BookId { get; set; }

        [DynamoDBProperty]
        public string UserId { get; set; }

        [DynamoDBProperty]
        public int Rating { get; set; }

        [DynamoDBProperty]
        public string Comment { get; set; }

        [DynamoDBProperty]
        public DateTime CreatedAt { get; set; }
    }
} 