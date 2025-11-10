import { createFileRoute } from "@tanstack/react-router";
import { useWeatherForecastGetHook } from "~/gen";

export const Route = createFileRoute("/popular/")({
    component: RouteComponent,
});

function RouteComponent() {
    const { data } = useWeatherForecastGetHook();
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
