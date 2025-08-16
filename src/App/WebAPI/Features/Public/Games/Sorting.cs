namespace Public.Games;

public static class Sorting
{
    public static string SortFilerOne = """

                                                    (
                                                        -- Normalize total_rating_count to 0-1
                                                        ((data->>'total_rating_count')::numeric / NULLIF(MAX((data->>'total_rating_count')::numeric) OVER (),0)) * 0.4

                                                        -- Normalize total_rating (assuming 0-100 scale)
                                                        + ((data->>'total_rating')::numeric / 100) * 0.4

                                                        -- Small boost for newer releases
                                                        + (1.0 / (EXTRACT(EPOCH FROM (NOW() - (data->>'release_date')::date)) + 1)) * 0.2
                                                    ) DESC
                                                
                                        """;

    public static string SortFilterTwo = """
                                         ((data->>'total_rating_count')::numeric * 0.7
                                                         + (data->>'total_rating')::numeric * 10) DESC,
                                                           ((data->>'rating_count')::numeric * 0.3
                                                         + (data->>'rating')::numeric * 10) DESC
                                         """;
}