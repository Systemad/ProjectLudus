import './App.css';
import { MantineProvider } from '@mantine/core';
// Import styles of packages that you've installed.
// All packages except `@mantine/hooks` require styles imports
import '@mantine/core/styles.css';
import { createFileRoute } from '@tanstack/react-router';
import { api } from '../main';

export const Route = createFileRoute("/weather")({
    component: Weather
})


function Weather() {

    const {isPending, isError, data, error } = api.weatherForecast.getWeatherForecast.useQuery();

  if (isPending) {
    return <span>Loading...</span>
  }

  if (isError) {
    return <span>Error</span>
  }
    return (
        <MantineProvider>
            <div>
 
                    <h1 id="tableLabel">Weather forecast</h1>
                    <p>This component demonstrates fetching data from the server.</p>
                    <table className="table table-striped" aria-labelledby="tableLabel">
                    <thead>
                        <tr>
                            <th>Date</th>
                            <th>Temp. (C)</th>
                            <th>Temp. (F)</th>
                            <th>Summary</th>
                        </tr>
                    </thead>
                    <tbody>
                        {data.map(forecast =>
                            <tr key={forecast.date}>
                                <td>{forecast.date}</td>
                                <td>{forecast.temperatureC}</td>
                                <td>{forecast.temperatureF}</td>
                                <td>{forecast.summary}</td>
                            </tr>
                        )}
                    </tbody>
                    </table>
  
            </div>
        </MantineProvider>
    );

}