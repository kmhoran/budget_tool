using System;

namespace Common.Core.Models
{
    public class Categories
    {
        // TODO: it would be better not to rely on user-specific implementation
        public CategoriesPersonal Green { get; set; }
        public CategoriesPersonal Red { get; set; }
    }
}