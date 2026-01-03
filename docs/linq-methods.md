# LINQ Methods Used in MasteringLinq

This document lists the LINQ methods used across this solution, with a short description, a compact usage example (C#), and a typical use case.

--

Select
- Description: Projects each element of a sequence into a new form.
- Example: `var names = tracks.Select(t => t.Name);`
- Use case: Transform entities into DTOs or extract a single property.

SelectMany
- Description: Flattens nested collections into a single sequence.
- Example: `var allTracks = playlists.SelectMany(p => p.PlaylistTracks.Select(pt => pt.Track));`
- Use case: Flatten one-to-many relationships for easier processing.

Where
- Description: Filters a sequence based on a predicate.
- Example: `var longTracks = tracks.Where(t => t.Milliseconds > 180000);`
- Use case: Filter results returned from DB or in-memory collections.

Join
- Description: Inner-join two sequences based on matching keys.
- Example: `var q = playlistTracks.Join(tracks, pt => pt.TrackId, t => t.TrackId, (pt,t) => t);`
- Use case: Combine related sets (e.g., join foreign-key relationships).

GroupJoin
- Description: Correlates elements of two sequences and groups the results (left outer join style).
- Example: `albums.GroupJoin(tracks, a => a.AlbumId, t => t.AlbumId, (a, ts) => new { a, Tracks = ts })`
- Use case: Build parent records with collections of child records.

GroupBy
- Description: Groups elements that share a common key.
- Example: `var grouped = tracks.GroupBy(t => t.Album?.Title);`
- Use case: Produce aggregates per key (counts, sums, top-N per group).

OrderBy / OrderByDescending
- Description: Sorts elements by a key (ascending/descending).
- Example: `tracks.OrderBy(t => t.Name)` / `groups.OrderByDescending(g => g.Count())`
- Use case: Present ordered results or prepare for `First`/`Last` semantics.

ThenBy / ThenByDescending
- Description: Secondary ordering after an initial OrderBy.
- Example: `artists.OrderBy(a => a.Name).ThenByDescending(a => a.Country)`
- Use case: Multi-column sorting (stable tie-breakers).

Take / Skip
- Description: `Take(n)` returns first n elements; `Skip(n)` bypasses first n elements.
- Example: `tracks.Take(10)` / `tracks.Skip(5)`
- Use case: Pagination, sampling, or limiting results.

TakeWhile / SkipWhile
- Description: Conditional partitioning while a predicate holds (based on sequence order).
- Example: `items.TakeWhile(x => x.Score > 50)`
- Use case: Consume prefix/suffix of ordered sequences.

First / FirstOrDefault
- Description: Returns the first element (or default if none for FirstOrDefault).
- Example: `var f = tracks.First(t => t.TrackId == 1);`
- Use case: Retrieve a single element expected at the start of a sequence.

Last / LastOrDefault
- Description: Returns the last element (or default if none for LastOrDefault).
- Example: `var last = tracks.OrderBy(t => t.TrackId).Last();`
- Use case: Get the final item after ordering.

Single / SingleOrDefault
- Description: Returns the single element matching a predicate (throws if multiple). SingleOrDefault returns default when empty.
- Example: `var s = tracks.Single(t => t.TrackId == 1);`
- Use case: Validate uniqueness or fetch unique records by key.

ElementAt / ElementAtOrDefault
- Description: Gets element at specified zero-based index.
- Example: `var e = tracks.ElementAt(5);`
- Use case: Index-based access on sequences.

Any / All
- Description: `Any` checks if any elements satisfy a predicate; `All` checks if all do.
- Example: `tracks.Any(t => t.Name.Length > 50)` / `tracks.All(t => t.Composer != null)`
- Use case: Quick existence/validation checks.

Contains
- Description: Tests whether a sequence contains a specified element (or key when used on projected collection).
- Example: `trackNames.Contains("Imagine")`
- Use case: Membership tests, de-dup checks.

Count
- Description: Counts elements in a sequence (optionally with predicate).
- Example: `var n = tracks.Count();` / `group.Count()`
- Use case: Aggregation and reporting of totals.

Sum / Min / Max / Average
- Description: Numeric aggregations across a sequence.
- Example: `tracks.Sum(t => t.Milliseconds)` / `tracks.Average(t => t.Milliseconds)`
- Use case: Metrics like totals, min/max durations, averages.

Aggregate
- Description: Applies an accumulator function over a sequence to produce a single value.
- Example: `names.Aggregate((cur, next) => cur + ", " + next)`
- Use case: Custom reductions (concatenation or fold operations).

Distinct
- Description: Removes duplicate elements.
- Example: `var unique = ids.Distinct();`
- Use case: De-dup sets, normalization before set operations.

Except / Intersect / Union / Concat
- Description: Set operations: `Except` (A - B), `Intersect` (A ∩ B), `Union` (A ∪ B), `Concat` (append sequences).
- Example: `a.Except(b)` / `a.Intersect(b)` / `a.Union(b)` / `a.Concat(b)`
- Use case: Compare playlists, merge ID lists, find unique vs common items.

ToList / ToListAsync / ToDictionary
- Description: Materialize sequences into collections or dictionaries. `ToListAsync` is used with EF Core async queries.
- Example: `var list = await query.ToListAsync();` / `var dict = items.ToDictionary(x => x.Id);`
- Use case: Materialize query results for iteration, further in-memory processing, or passing to APIs.

AsParallel (PLINQ)
- Description: Enables parallel LINQ execution on in-memory sequences.
- Example: `var parallelGrouped = tracks.AsParallel().Where(...).GroupBy(...).ToList();`
- Use case: CPU-bound bulk processing on collections where parallelism helps.

AsQueryable / AsEnumerable
- Description: Control where a query is executed/translated (deferred execution / EF translation boundaries).
- Example: `query.AsEnumerable()` to force client-side processing.
- Use case: When part of a query cannot be translated to SQL (EF Core) and you need explicit client evaluation.

Join/GroupJoin notes
- `Join` produces flattened results (inner join). `GroupJoin` produces parent with child collections. Both are used in this project for combining playlists, tracks, albums, invoices.

Practical tips from this project
- Use `ToListAsync()` when executing EF Core queries asynchronously.
- Prefer `Where` + `Select` projection in the database (avoid client-side evaluation) unless you explicitly call `AsEnumerable()`/`ToList()`.
- Use `GroupBy` on DB only for simple translations; complex projections sometimes require client-side materialization.

--

If you'd like, I can:
- Add code snippets taken directly from the repository for each method (contextual examples).
- Generate a blog-ready markdown file with formatted sections and links to the relevant source files.

Created from scanning `src/` of this repository on January 3, 2026.
