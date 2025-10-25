namespace OpenLlamaAPI.Ai;

public static class Prompts
{
    public static string WeatherPrompt =
        """
        The submitted image is a livecam snapshot of Hamburg. Give a structured report of:
        1. Describe the daytime in a short sentence with one of the following states: dawn, morning, noon, forenoon, afternoon, evening, dusk, night.
        2. Describe the weather on the image as far as it is possible in maximum two sentences.
        """;
}