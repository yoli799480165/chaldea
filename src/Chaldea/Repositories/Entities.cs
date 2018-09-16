using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chaldea.Repositories
{
    public class Bangumi : IEntity<string>
    {
        public string Name { get; set; }
        public List<Anime> Animes { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }

    public class Anime : IEntity<string>
    {
        public string Title { get; set; }

        public string Cover { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }

    public class AnimeDetail : IEntity<string>
    {
        public string AnimeId { get; set; }

        public string Desc { get; set; }

        public string State { get; set; }

        public string Type { get; set; }

        public List<string> Tags { get; set; }

        public List<Resource> Animes { get; set; }

        public List<Resource> Comics { get; set; }

        public List<Resource> Novels { get; set; }

        public List<Comment> Comments { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }

    public class Resource
    {
        public string Uid { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public MediaMetaData MetaData { get; set; }
    }

    public class MediaMetaData
    {
        public int Duration { get; set; }
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }
        public int FrameRate { get; set; }
    }

    public class Comment : IEntity<string>
    {
        public string Content { get; set; }

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

    public class User : IEntity<string>
    {
        public string Uid { get; set; }

        public string NickName { get; set; }

        public string Name { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }

        public string ProfilePhoto { get; set; }

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