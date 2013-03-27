using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace tracksStackd
{
    public class RestQuery
    {
        public string Sort { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class QueryResponse : IHasResponseStatus
    {
        public ResponseStatus ResponseStatus { get; set; }
        public List<Link> Links { get; set; }
    }

    public class QueryRequestFilterAttribute : Attribute, IHasRequestFilter
    {
        public IHasRequestFilter Copy()
        {
            return this;
        }

        public int Priority
        {
            get { return -100; }
        }

        public void RequestFilter(IHttpRequest req, IHttpResponse res, object requestDto)
        {
            var request = req as RestQuery;
            if (request == null) return;
            request.PageNumber = string.IsNullOrEmpty(req.QueryString["pageNumber"]) ? 1 : int.Parse(req.QueryString["pageNumber"]);
            request.PageSize = string.IsNullOrEmpty(req.QueryString["pageSize"]) ? 1 : int.Parse(req.QueryString["pageSize"]);
            request.Sort = string.IsNullOrEmpty(req.QueryString["sort"]) ? "id" : req.QueryString["sort"];
        }
    }
}
