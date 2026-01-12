#!/usr/bin/env python3
import json
import os
import re
import subprocess
import sys
from subprocess import CompletedProcess
from typing import Annotated, List, Optional

import typer

app = typer.Typer()


def clean_dbt_output2(raw_output: str) -> str:
    lines: list[str] = raw_output.splitlines()
    clean_lines: list[str] = []

    for line in lines:
        cleaned = re.sub(r"\x1b\[[0-9;]*m", "", line)
        stripped = cleaned.strip()

        if stripped and not any(
            [
                stripped.startswith("Running with dbt="),
                stripped.startswith("Registered adapter:"),
                stripped.startswith("Found"),
                "Running with dbt=" in stripped,
                "Registered adapter:" in stripped,
                "Found" in stripped,
                stripped.startswith("version")
                and (
                    "models" in stripped or "tests" in stripped or "sources" in stripped
                ),
            ]
        ):
            clean_lines.append(cleaned.rstrip())

    return "\n".join(clean_lines) + "\n"


# def clean_dbt_output(raw_output: str) -> str:
#    lines: list[str] = raw_output.splitlines()
#    clean_lines: list[str] = []
#
#    for line in lines:
#        stripped_line: str = line.strip()
#
#        if stripped_line and not any(
#            [
#                stripped_line.startswith("Running with dbt="),
#                stripped_line.startswith("Registered adapter:"),
#                stripped_line.startswith("Found"),
#                "Running with dbt=" in stripped_line,
#                "Registered adapter:" in stripped_line,
#                "Found" in stripped_line,
#                stripped_line.startswith("version")
#                and (
#                    "models" in stripped_line
#                    or "tests" in stripped_line
#                    or "sources" in stripped_line
#                ),
#                stripped_line.startswith("\x1b[0m"),  # ANSI color codes
#            ]
#        ):
#            if stripped_line:
#                clean_lines.append(stripped_line)
#
#    return "\n".join(clean_lines)


def get_model_names(select_path: str) -> list[str]:
    try:
        result: CompletedProcess[str] = subprocess.run(
            ["dbt", "ls", "--select", f"{select_path}", "--output", "name"],
            capture_output=True,
            text=True,
            check=True,
        )

        # clean_output: str = clean_dbt_output(result.stdout)
        clean_output: str = clean_dbt_output2(result.stdout)
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
            json.dumps({"model_names": [model]}),
        ],
        capture_output=True,
        text=True,
        check=True,
    )

    return result.stdout


def execute_sql_model_command(model: str, source_name: str) -> str:
    result: CompletedProcess[str] = subprocess.run(
        [
            "dbt",
            "run-operation",
            "generate_base_model",
            "--args",
            json.dumps({"source_name": source_name, "table_name": model}),
        ],
        capture_output=True,
        text=True,
        check=True,
    )

    return result.stdout


def generate_model_yaml(model_names: list[str], output_dir: str, prefix: str):
    if not model_names:
        print("No models found to generate documentation for.")
        return

    try:
        for model_name in model_names:
            print(f"Generating Model: {model_name}")
            output_file: str = os.path.join(output_dir, f"{prefix}{model_name}.yml")
            command_result: str = execute_yml_model_command(model_name)
            clean_yaml: str = clean_dbt_output2(command_result)

            with open(output_file, "w") as f:
                f.write(clean_yaml)
    except subprocess.CalledProcessError as e:
        print(f"Error running dbt run-operation: {e}", file=sys.stderr)
        print(f"dbt command failed (exit code {e.returncode}):", file=sys.stderr)
        print(f"STDOUT: {e.stdout}", file=sys.stderr)
        print(f"STDERR: {e.stderr}", file=sys.stderr)
        sys.exit(1)


def generate_model_sql(
    model_names: list[str], output_dir: str, prefix: str, source_name: str
):
    if not model_names:
        print("No models found to generate documentation for.")
        return

    try:
        for model_name in model_names:
            print(f"Generating Model: {model_name}")
            output_file: str = os.path.join(output_dir, f"{prefix}{model_name}.sql")
            command_result: str = execute_sql_model_command(model_name, source_name)
            clean_yaml: str = clean_dbt_output2(command_result)

            with open(output_file, "w") as f:
                f.write(clean_yaml)
    except subprocess.CalledProcessError as e:
        print(f"Error running dbt run-operation: {e}", file=sys.stderr)
        print(f"dbt command failed (exit code {e.returncode}):", file=sys.stderr)
        print(f"STDOUT: {e.stdout}", file=sys.stderr)
        print(f"STDERR: {e.stderr}", file=sys.stderr)
        sys.exit(1)

    # print(f"Fetching models from: {select_path}")


@app.command()
def main(
    prefix: Annotated[Optional[str], typer.Option("--prefix", help="Override")] = None,
    yml_prefix: str = typer.Option("_", "--yml-prefix"),
    sql_prefix: Optional[str] = typer.Option(
        None, "--sql-prefix"
    ),  # Default handled in logic
    layer: str = typer.Option("marts", "--layer"),
    source_name: Annotated[Optional[str], typer.Option("--source-name")] = None,
    output_type: str = typer.Option("sql", "--type"),
    models: Annotated[Optional[List[str]], typer.Option("--model")] = None,
):
    # 1. Handle Layer and SQL Prefix Logic
    if layer == "staging":
        s_pref = "stg_"
    else:
        # If user didn't provide --prefix or --sql-prefix, default to mart_
        s_pref = prefix or sql_prefix or "mart_"

    y_pref = prefix or yml_prefix
    select_path = f"models/{layer}"

    # 2. Get Model Names
    model_names = models if models else get_model_names(select_path)

    if not model_names:
        typer.echo(f"No models found in {select_path}")
        raise typer.Exit()

    typer.echo(
        f"Generating {output_type} for {layer} (Prefix: {s_pref if output_type == 'sql' else y_pref})"
    )

    # 3. Execution Logic
    if output_type == "sql":
        if not source_name:
            raise typer.BadParameter("--source-name is required for SQL format")
        generate_model_sql(model_names, select_path, s_pref, source_name)

    elif output_type == "yml":
        generate_model_yaml(model_names, select_path, y_pref)


if __name__ == "__main__":
    app()
