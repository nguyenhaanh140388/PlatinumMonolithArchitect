namespace Platinum.Core.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// SortFilterBaseRequest.
    /// </summary>
    public class SortFilterBaseRequest
    {
        /// <summary>
        /// The default filter.
        /// </summary>
        private const string DefaultFilter = "[]";

        /// <summary>
        /// The default sort.
        /// </summary>
        private const string DefaultSort = "{}";

        /// <summary>
        /// The default multisort.
        /// </summary>
        private const string DefaultMultiSort = "[]";

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>
        /// The filter.
        /// </value>
        public string Filter { get; set; } = DefaultFilter;

        /// <summary>
        /// Gets the filters.
        /// </summary>
        /// <value>
        /// The filters.
        /// </value>
        [JsonIgnore]
        public IList<Filter> Filters
        {
            get
            {
                try
                {
                    return JsonConvert.DeserializeObject<List<Filter>>(Filter);
                }
                catch
                {
                    return JsonConvert.DeserializeObject<Filter[]>(DefaultFilter);
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="Filter"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="Filter"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public Filter this[string key]
        {
            get
            {
                return Filters != null && Filters.Count > 0 ?
                Filters.FirstOrDefault(f => f.Prop.ToLower() == key.ToLower()) : null;
            }
        }

        /// <summary>
        /// Gets or sets the sort.
        /// </summary>
        /// <value>
        /// The sort.
        /// </value>
        public string Sort { get; set; } = DefaultSort;

        /// <summary>
        /// Gets or sets the multi sort.
        /// </summary>
        /// <value>
        /// The multi sort.
        /// </value>
        public string MultiSort { get; set; } = DefaultMultiSort;

        /// <summary>
        /// Gets the sorts.
        /// </summary>
        /// <value>
        /// The sorts.
        /// </value>
        [JsonIgnore]
        public IList<Sort> Sorts
        {
            get
            {
                try
                {
                    return JsonConvert.DeserializeObject<List<Sort>>(MultiSort);
                }
                catch
                {
                    return JsonConvert.DeserializeObject<List<Sort>>(DefaultMultiSort);
                }
            }
        }

        /// <summary>
        /// Gets the sort logic.
        /// </summary>
        /// <value>
        /// The sort logic.
        /// </value>
        [JsonIgnore]
        public ISort SortLogic
        {
            get
            {
                try
                {
                    return JsonConvert.DeserializeObject<Sort>(Sort);
                }
                catch
                {
                    return JsonConvert.DeserializeObject<Sort>(DefaultSort);
                }
            }
        }
    }
}
