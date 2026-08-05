export function parseYearParam(value: string | string[] | undefined, fallbackYear: number) {
  if (typeof value !== "string" || !/^\d{1,4}$/.test(value)) {
    return fallbackYear;
  }

  const year = Number(value);
  return Number.isInteger(year) && year >= 1 && year <= 9999 ? year : fallbackYear;
}
