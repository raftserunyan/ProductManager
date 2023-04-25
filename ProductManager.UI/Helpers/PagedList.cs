﻿using System.Collections.Generic;

namespace ProductManager.UI.Helpers
{
    public class PagedList<T>
    {
        public int PageIndex { get; set; }
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public List<T> Items { get; set; }
    }
}
