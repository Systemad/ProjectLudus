#!/usr/bin/env python3
import json
import os
import subprocess
import sys
from subprocess import CompletedProcess

import click


def clean_dbt_output(raw_output: str) -> str:
    lines: list[str] = raw_output.splitlines()
    clean_lines: list[str] = []

    for line in lines:
        stripped_line: str = line.strip()

        if stripped_line and not any(
            [
                stripped_line.startswith("Running with dbt="),
                stripped_line.startswith("Registered adapter:"),
                stripped_line.startswith("Found"),
                "Running with dbt=" in stripped_line,
                "Registered adapter:" in stripped_line,
                "Found" in stripped_line,
                stripped_line.startswith("version")
                and (
                    "models" in stripped_line
                    or "tests" in stripped_line
                    or "sources" in stripped_line
                ),
                stripped_line.startswith("\x1b[0m"),  # ANSI color codes
            ]
        ):
            if stripped_line:
                clean_lines.append(stripped_line)

    return "\n".join(clean_lines)


def get_model_names(select_path: str) -> list[str]:
    try:
        result: CompletedProcess[str] = subprocess.run(
            ["dbt", "ls", "--select", f"{select_path}", "--output", "name"],
            capture_output=True,
            text=True,
            check=True,
        )

        clean_output: str = clean_dbt_output(result.stdout)
        model_names: list[str] = clean_output.splitlines()
        return model_names

    except subprocess.CalledProcessError as e:
        print(f"Error running dbt ls: {e.stderr}", file=sys.stderr)
        sys.exit(1)
    except FileNotFoundError:
        print(
            "Error: dbt command not found. Make sure dbt is installed and in your PATH.",
            file=sys.stderr,
        )
        sys.exit(1)


def execute_yml_model_command(model: str) -> str:
    result: CompletedProcess[str] = subprocess.run(
        [
            "dbt",
            "run-operation",
            "generate_model_yaml",
            "--args",
            json.dumps({"model_names": model}),
        ],
        capture_output=True,
        text=True,
        check=True,
    )

    return result.stdout


def generate_model_yaml(model_names: list[str], output_dir: str):
    if not model_names:
        print("No models found to generate documentation for.")
        return

    try:
        for model_name in model_names:
            print(f"Generating Model: {model_name}")
            output_file: str = os.path.join(output_dir, f"_{model_name}.yml")
            command_result: str = execute_yml_model_command(model_name)
            clean_yaml: str = clean_dbt_output(command_result)

            with open(output_file, "w") as f:
                f.write(clean_yaml)
    except subprocess.CalledProcessError as e:
        print(f"Error running dbt run-operation: {e}", file=sys.stderr)
        sys.exit(1)


@click.command()
@click.option("--prefix", default="_", help="Prefix")
@click.option("--layer", default="marts", help="Prefix")
def main(prefix: str, layer: str):
    select_path: str = f"models/{layer}"
    # print(f"Fetching models from: {select_path}")

    model_names: list[str] = get_model_names(select_path)
    generate_model_yaml(model_names, select_path)
    for name in model_names:
        print(f"Model: {name}")

    if not model_names:
        print(f"No models found in {select_path}")
        sys.exit(0)

    # print(model_names[2])
    # Generate documentation


#    generate_model_yaml(model_names, select_path)


if __name__ == "__main__":
    main()
