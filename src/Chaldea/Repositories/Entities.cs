using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chaldea.Repositories
{
    /// <summary>
    ///     动漫
    /// </summary>
    public class Anime
    {
        /// <summary>
        ///     Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     封面图片（指向资源ID还是直接使用文件名?）
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        ///     播放量
        /// </summary>
        public long PlayCounts { get; set; }

        /// <summary>
        ///     订阅量
        /// </summary>
        public long SubCounts { get; set; }

        /// <summary>
        ///     描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        ///     作者
        /// </summary>
        public string Auth { get; set; }

        /// <summary>
        ///     出品方
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        ///     监督
        /// </summary>
        public string Director { get; set; }

        /// <summary>
        ///     状态（连载/完结）
        /// </summary>
        public string State { get; set; }

        /// <summary>
        ///     类型（TV）
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     分级
        /// </summary>
        public int Level { get; set; }

        public List<string> Tags { get; set; }

        public List<string> Videos { get; set; }

        public List<string> Comics { get; set; }

        public List<string> Novels { get; set; }

        public List<string> Comments { get; set; }
    }

    /// <summary>
    ///     番组分类
    /// </summary>
    public class Bangumi : IEntity<string>
    {
        public string Name { get; set; }

        public List<Anime> Animes { get; set; }

        public string Id { get; set; }
    }

    /// <summary>
    ///     视频资源
    /// </summary>
    public class Video : IEntity<string>
    {
        public string Title { get; set; }

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

    /// <summary>
    ///     漫画资源
    /// </summary>
    public class Comic : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }

    /// <summary>
    ///     小说资源
    /// </summary>
    public class Novel : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }

    /// <summary>
    ///     评论
    /// </summary>
    public class Comment : IEntity<string>
    {
        public string Content { get; set; }

        public string UserId { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }

    /// <summary>
    ///     Banner
    /// </summary>
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