import { defineConfig } from "vite-plus";

export default defineConfig({
    staged: {
        "*": "vp check --fix",
    },
    lint: { options: { typeAware: true, typeCheck: true } },
    fmt: {
        tabWidth: 4,
        semi: true,
    },
});
