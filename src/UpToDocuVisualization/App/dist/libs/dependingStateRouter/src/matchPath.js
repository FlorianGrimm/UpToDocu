import { pathToRegexp } from "path-to-regexp";
const cache = {};
const cacheLimit = 100;
//let cacheCount = 0;
function compilePath(path, options) {
    const cacheKey = `${options.end}${options.strict}${options.sensitive}`;
    let pathCache = cache[cacheKey] || (cache[cacheKey] = {});
    if (pathCache[path])
        return pathCache[path];
    const keys = [];
    const regexp = pathToRegexp(path, keys, options);
    const result = { regexp, keys };
    if (Object.keys(pathCache).length >= cacheLimit) {
        pathCache = (cache[cacheKey] = {});
    }
    // if (cacheCount < cacheLimit) {
    //     pathCache[path] = result;
    //     cacheCount++;
    // }
    pathCache[path] = result;
    return result;
}
/**
 * Public API for matching a URL pathname to a path.
 */
export function matchPath(pathname, options) {
    if (typeof options === "string" || Array.isArray(options)) {
        options = { path: options };
    }
    const { path, exact = false, strict = false, sensitive = false } = options;
    const paths = [].concat(path);
    const r = (matched, path) => {
        if (!path && path !== "")
            return null;
        if (matched)
            return matched;
        const { regexp, keys } = compilePath(path, {
            end: exact,
            strict,
            sensitive
        });
        const match = regexp.exec(pathname);
        if (!match)
            return null;
        const [url, ...values] = match;
        const isExact = pathname === url;
        if (exact && !isExact)
            return null;
        const result = {
            path,
            url: path === "/" && url === "" ? "/" : url,
            isExact,
            params: keys.reduce((memo, key, index) => {
                memo[key.name] = values[index];
                return memo;
            }, {})
        };
        return result;
    };
    return paths.reduce(r, null);
}
export default matchPath;
