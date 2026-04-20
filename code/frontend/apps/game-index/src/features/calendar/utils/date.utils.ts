import { dayjs } from "@src/utils/dayjs";

export const DAYS_PER_WEEK = 7;
export const MAX_GAMES_PER_DAY = 5;
export const MAX_WEEKS = 52;

export function getOrdinal(value: number) {
    const mod10 = value % 10;
    const mod100 = value % 100;

    if (mod10 === 1 && mod100 !== 11) return "st";
    if (mod10 === 2 && mod100 !== 12) return "nd";
    if (mod10 === 3 && mod100 !== 13) return "rd";

    return "th";
}

export function formatHeadingLabel(epoch: number) {
    const date = dayjs.unix(epoch).utc();
    const day = date.date();

    return `${date.format("MMMM D")}${getOrdinal(day)}`;
}

export function getNextStartDate(currentStartDate: string): string {
    return dayjs.utc(currentStartDate).add(DAYS_PER_WEEK, "day").format("YYYY-MM-DD");
}
