using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chaldea.Repositories
{
    /// <summary>
    ///     番组分类
    /// </summary>
    public class Bangumi : IEntity<string>
    {
        public string Name { get; set; }

        public List<string> Animes { get; set; }

        public string Id { get; set; }
    }

    /// <summary>
    ///     动漫
    /// </summary>
    public class Anime : IEntity<string>
    {
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

        /// <summary>
        ///     标签
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        ///     视频资源
        /// </summary>
        public List<string> Videos { get; set; }

        /// <summary>
        ///     漫画资源
        /// </summary>
        public List<string> Comics { get; set; }

        /// <summary>
        ///     小说资源
        /// </summary>
        public List<string> Novels { get; set; }

        /// <summary>
        ///     评论
        /// </summary>
        public List<string> Comments { get; set; }

        /// <summary>
        ///     Id
        /// </summary>
        public string Id { get; set; }
    }

    /// <summary>
    ///     动漫标签
    /// </summary>
    public class AnimeTag : IEntity<string>
    {
        public List<string> Types { get; set; }

        public List<string> Tags { get; set; }

        public List<string> States { get; set; }

        public string Id { get; set; }
    }

    /// <summary>
    ///     视频资源
    /// </summary>
    public class Video : IEntity<string>
    {
        /// <summary>
        ///     文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///     文件大小
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        ///     显示标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     链接地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///     视频时长
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        ///     帧宽
        /// </summary>
        public int FrameWidth { get; set; }

        /// <summary>
        ///     帧高
        /// </summary>
        public int FrameHeight { get; set; }

        /// <summary>
        ///     帧率
        /// </summary>
        public int FrameRate { get; set; }

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

    /// <summary>
    ///     成就
    /// </summary>
    public class Achievement : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }

    /// <summary>
    ///     图片资源
    /// </summary>
    public class Image : IEntity<string>
    {
        public string MetaData { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }

    /// <summary>
    ///     节点
    /// </summary>
    public class Node : IEntity<string>
    {
        /// <summary>
        ///     节点名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     系统类型
        /// </summary>
        public string OsType { get; set; }

        /// <summary>
        ///     对外IP地址
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        ///     状态
        /// </summary>
        public NodeState State { get; set; }

        /// <summary>
        ///     会话Id
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        ///     Id
        /// </summary>
        public string Id { get; set; }
    }

    /// <summary>
    ///     节点状态
    /// </summary>
    public enum NodeState
    {
        Offline = 0,
        Online = 1
    }
}