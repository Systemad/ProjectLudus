import { createFileRoute } from "@tanstack/react-router";
import { useWeatherForecastGet } from "~/api";

export const Route = createFileRoute("/popular/")({
    component: RouteComponent,
});

function RouteComponent() {
    const { data } = useWeatherForecastGet();
    return (
        <div>
            {data?.map((forecast) => (
                <div key={forecast.date}>
                    <div>{forecast.date}</div>
                    <div>{forecast.summary}</div>
                    <div>{forecast.temperatureC}</div>
                    <div>{forecast.temperatureF}</div>
                </div>
            ))}
        </div>
    );
}
