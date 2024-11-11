﻿using SalesAPI.Application.Commons.Enums;

namespace SalesAPI.Application.Commons
{
    public class PaginationRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Guid? Id { get; set; }
        public string? SearchText { get; set; }
        public ListOrder OrderBy { get; set; }
        public bool? Descending { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public PaginationRequest()
        {
            PageNumber = 1;
            PageSize = 10;
        }
        public PaginationRequest(int pageNumber, int pageSize, bool descending = true)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize > 10 ? 10 : pageSize;
        }


    }
}
