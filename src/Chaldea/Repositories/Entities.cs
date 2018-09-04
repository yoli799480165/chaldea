using System.Collections.Generic;
using Chaldea.Services;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Chaldea.Repositories
{
    public class Bangumi : IEntity<ObjectId>
    {
        public string Name { get; set; }
        public List<Anime> Animes { get; set; }
        [BsonId] public ObjectId Id { get; set; }
    }

    public class Anime : IEntity<ObjectId>
    {
        public string Title { get; set; }

        public string Cover { get; set; }

        [JsonConverter(typeof(ObjectIdConverter))]
        [BsonId] public ObjectId Id { get; set; }
    }

    public class AnimeDetail : IEntity<ObjectId>
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
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
    }

    public class Resource
    {
        public string Uid { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }
    }

    public class Comment : IEntity<ObjectId>
    {
        public string Content { get; set; }

        public ObjectId Id { get; set; }
    }

    public class Banner : IEntity<ObjectId>
    {
        public string Image { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public ObjectId Id { get; set; }
    }

    public class User : IEntity<ObjectId>
    {
        public string Uid { get; set; }

        public string NickName { get; set; }

        public string Name { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }

        public string ProfilePhoto { get; set; }

        public ObjectId Id { get; set; }
    }

    public class Achievement : IEntity<ObjectId>
    {
        public ObjectId Id { get; set; }
    }

    public class Image : IEntity<ObjectId>
    {
        public string MetaData { get; set; }

        public ObjectId Id { get; set; }
    }
}