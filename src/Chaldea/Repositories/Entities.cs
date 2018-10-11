using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chaldea.Repositories
{
    public class Anime
    {
        public string AnimeId { get; set; }

        public string Title { get; set; }

        public string Cover { get; set; }

        public long PlayCounts { get; set; }

        public long SubCounts { get; set; }

        public string Desc { get; set; }

        public string Auth { get; set; }

        public string Publisher { get; set; }

        public string Director { get; set; }

        public string State { get; set; }

        public string Type { get; set; }

        public int Level { get; set; }

        public List<string> Tags { get; set; }

        public List<string> Videos { get; set; }

        public List<string> Comics { get; set; }

        public List<string> Novels { get; set; }

        public List<string> Comments { get; set; }
    }

    public class Bangumi : IEntity<string>
    {
        public string Name { get; set; }

        public List<Anime> Animes { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }

    public class Video : IEntity<string>
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public int Duration { get; set; }

        public int FrameWidth { get; set; }

        public int FrameHeight { get; set; }

        public int FrameRate { get; set; }

        public long Length { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }

    public class Comic : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }

    public class Novel : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }

    public class Comment : IEntity<string>
    {
        public string Content { get; set; }

        public string UserId { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }

    public class Banner : IEntity<string>
    {
        public string Image { get; set; }

        public string Link { get; set; }

        public string Title { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }

    public class Achievement : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }

    public class Image : IEntity<string>
    {
        public string MetaData { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}