import { useEffect } from "react";
import * as CookieConsent from "vanilla-cookieconsent";
import pluginConfig from "./CookieConsentConfig";
import "vanilla-cookieconsent/dist/cookieconsent.css";

export function CookieConsentComponent() {
    useEffect(() => {
        CookieConsent.run(pluginConfig);
    }, []);

    return null;
}