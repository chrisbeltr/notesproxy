#!/usr/bin/env bash
set -euo pipefail

TARGET_DIR="$HOME/.local/bin"
COMPLETION_DIR="$HOME/.local/share/bash-completion/completions"
PUBLISH_DIR="src/NotesProxy.Cli/bin/publish"
SERVER_PUBLISH_DIR="src/NotesProxy.Server/bin/publish"

dotnet publish src/NotesProxy.Cli -r linux-x64 -c Release -o "$PUBLISH_DIR"
dotnet publish src/NotesProxy.Server -r linux-x64 -c Release -o "$SERVER_PUBLISH_DIR"

mkdir -p "$TARGET_DIR"
cp "$PUBLISH_DIR/notesproxy" "$TARGET_DIR/notesproxy"
chmod +x "$TARGET_DIR/notesproxy"
cp "$TARGET_DIR/notesproxy" "$TARGET_DIR/np"
cp "$SERVER_PUBLISH_DIR/notesproxy-server" "$TARGET_DIR/notesproxy-server"
chmod +x "$TARGET_DIR/notesproxy-server"

mkdir -p "$COMPLETION_DIR"
cp "completion/completely.bash" "$COMPLETION_DIR/notesproxy"
cp "completion/completely.bash" "$COMPLETION_DIR/np"

if [[ ":$PATH:" != *":$TARGET_DIR:"* ]]; then
    RC_FILE="$HOME/.bashrc"
    [[ -n "${ZSH_VERSION:-}" ]] && RC_FILE="$HOME/.zshrc"
    if ! grep -qs "$TARGET_DIR" "$RC_FILE" 2>/dev/null; then
        echo "export PATH=\"$TARGET_DIR:\$PATH\"" >> "$RC_FILE"
    fi
    echo "Added '$TARGET_DIR' to PATH in $RC_FILE. Restart your terminal or run: source $RC_FILE"
else
    echo "'$TARGET_DIR' already in PATH."
fi
