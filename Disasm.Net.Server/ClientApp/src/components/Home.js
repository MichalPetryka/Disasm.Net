import React, { Component } from 'react';
import Editor from '@monaco-editor/react';

export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
        <Editor height="90vh" defaultLanguage="csharp" defaultValue="// some comment" />
    );
  }
}
