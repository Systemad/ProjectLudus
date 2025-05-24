const fs = require("fs");
const path = require("path");
const env = process.env;

const target = env.ASPNETCORE_HTTPS_PORT
  ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
  : env.ASPNETCORE_URLS
  ? env.ASPNETCORE_URLS.split(";")[0]
  : "https://localhost:7247";

fs.writeFileSync(
  path.join(__dirname, "../.env"),
  `VITE_API_BASE_URL=${target}\n`
);

