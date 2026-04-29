// oxlint-disable no-unused-vars
import type { CookieConsentConfig } from "vanilla-cookieconsent";

const pluginConfig: CookieConsentConfig = {
    guiOptions: {
        consentModal: {
            layout: "box",
            position: "bottom right",
            equalWeightButtons: true,
        },
        preferencesModal: {
            layout: "box",
            position: "left",
        },
    },

    onConsent: () => {},

    onChange: () => {},

    cookie: {
        name: "AnalyticsConsent",
    },

    categories: {
        necessary: {
            readOnly: true,
            enabled: true,
        },
        analytics: {
            autoClear: {
                cookies: [{ name: /^(_ga|_gid|_ga_.*)/ }],
            },
        },
    },

    language: {
        default: "en",
        translations: {
            en: {
                consentModal: {
                    title: "We use analytics cookies",
                    description:
                        "We use a single analytics cookie to understand how the site is used. It is only enabled if you allow it.",
                    acceptAllBtn: "Accept analytics",
                    acceptNecessaryBtn: "Reject",
                    showPreferencesBtn: "More options",
                    footer: `<a href="/privacy">Privacy Policy</a>`,
                },

                preferencesModal: {
                    title: "Analytics preferences",
                    savePreferencesBtn: "Save choice",
                    closeIconLabel: "Close",
                    sections: [
                        {
                            title: "Analytics",
                            description: "Helps us understand how visitors use the site.",
                            linkedCategory: "analytics",
                        },
                    ],
                },
            },
        },
    },
};

export default pluginConfig;
