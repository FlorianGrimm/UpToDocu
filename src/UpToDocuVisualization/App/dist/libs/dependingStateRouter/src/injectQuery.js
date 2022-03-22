/**
 * Adds query to location.
 * Utilises the search prop of location to construct query.
 */
export function injectQuery(location) {
    if (location && typeof location.query !== "undefined") {
        // Don't inject query if it already exists in history
        return location;
    }
    const searchQuery = location && location.search;
    if (typeof searchQuery !== 'string' || searchQuery.length === 0) {
        return {
            ...location,
            query: {}
        };
    }
    // Ignore the `?` part of the search string e.g. ?username=codejockie
    const search = searchQuery.substring(1);
    // Split the query string on `&` e.g. ?username=codejockie&name=Kennedy
    const queries = search.split('&');
    // Contruct query
    const query = queries.reduce((acc, currentQuery) => {
        // Split on `=`, to get key and value
        const [queryKey, queryValue] = currentQuery.split('=');
        return {
            ...acc,
            [queryKey]: queryValue
        };
    }, {});
    return {
        ...location,
        query
    };
}
