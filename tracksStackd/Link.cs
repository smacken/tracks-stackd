using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace tracksStackd
{
    /// <summary>
    /// Hyperlink to another related api
    /// </summary>
    public class Link
    {
        public string Rel { get; set; }
        public string Href { get; set; }
        public string Title { get; set; }

        public Link()
        { }

        public Link(string rel, string href, string title = "")
        {
            this.Rel = rel;
            this.Href = href;
            this.Title = title;
        }
    }

    public interface IHasHypermediaLinkResponse
    {
        List<Link> Links { get; set; }
    }

    public class HypermediaResponse : IHasResponseStatus
    {
        public List<Link> Links { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    public class HypermediaResponse<T> : IHasResponseStatus
    {
        public T Result { get; set; }
        public List<Link> Links { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
