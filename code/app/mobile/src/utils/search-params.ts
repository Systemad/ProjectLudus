export function parseYearParam(value: string | string[] | undefined, fallbackYear: number) {
  if (Array.isArray(value) || value === undefined || !/^\d{1,4}$/.test(value)) {
    return fallbackYear;
  }

  const year = Number(value);
  return Number.isInteger(year) && year >= 1 && year <= 9999 ? year : fallbackYear;
}
