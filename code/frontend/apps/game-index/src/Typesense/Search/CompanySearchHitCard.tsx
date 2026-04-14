import type { CompanySearchHit } from "./companySearchUtils";
import { CompanyHitCard } from "./CompanyHitCard";

type CompanySearchHitCardProps = {
    hit: CompanySearchHit;
};

export function CompanySearchHitCard({ hit }: CompanySearchHitCardProps) {
    return <CompanyHitCard hit={hit} />;
}
