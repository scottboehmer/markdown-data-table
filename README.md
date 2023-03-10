# Markdown Data Tables

Use files containing markdown tables to store tabular data. Names for these data files should follow the 
pattern `*.table.md` and use the `text/markdown` MIME type so that they can be recognized as markdown files.

## Why?

- Markdown tables use `|` as a seperator which is less likely to show up in data than a `,` or `;` and so 
  escaping and quote-wrapping can be avoided.
- The `|` separator also mimics the vertical line of a table so that a raw data file looks more like a table.
- By using markdown, the data file gets nice rendering in any tool that already supports markdown rendering.
- Columns can designate an alignment for when the file is viewed as rendered markdown.

## Examples

```md
| Name  | Hex     |
| :---- | :-----: |
| White | #FFFFFF |
| Black | #000000 |
| Red   | #FF0000 |
| Green | #00FF00 |
| Blue  | #0000FF |
```

```md
| Name                            | Level | Type         | Source                                          |
| :------------------------------ | :---: | :----------- | :---------------------------------------------- |
| Uthelyn the Mad                 | 8     | Skirmisher   | Monster Vault: Threats to the Nentir Vale, p.17 |
| Adrian "Iceheart" Reginold      | 8     | Controler    | Monster Vault: Threats to the Nentir Vale, p.18 |
| Madras Kalgore, The Poison Fist | 24    | Elite Lurker | Draconomicon: Chromatic Dragons, p.247          |
```

## Implementation

A quick implementation I've been using in a hobby project is included in this repo. It includes a simple
console app that can tidy up data files by adjusting column widths or display a certain row from a table.
It is not designed or cleaned up for general use, so I'm including it just as an example of the idea.

- To display a whole table: `MarkdownDataTool read --file <path to file>`
- To display a row from a table: `MarkdownDataTool read --file <path to file> --key <value>`
- To clean up a file: `MarkdownDataTool tidy --file <path to file>`
