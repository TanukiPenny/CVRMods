cvr.menu.prototype.CVUI = {
    info: function(){
        return {
            name: "ConsoleViewer",
            version_major: 0,
            version_minor: 1,
            description: "ConsoleViewer Menu",
            author: "Penny",
            author_id: "",
            external_links: [
            ],
            stylesheets: [
                {filename: "CVUI.css", modes: ["quickmenu"]},
                {filename: "bootstrap-grid.min.css", modes: ["quickmenu"]}
            ],
            compatibility: [],
            icon: "img/ConsoleViewer.png",
            feature_level: 1,
            supported_modes: ["quickmenu"]
        };
    },

    register: function(menu){
        console.log("Setting up CVUI");

        menu.templates["CVUI-icon"] = {c:"CVUI-icon", s: [{c: "icon"}], x: "CVUI-open", a: {"id" : "CVUI-IconBtn"}}
        menu.templates["CVUI-menu"] = {c: "CVUI menu-category hide", s: [
                {c: "container container-main", s: [
                    {c: "main-container", s: [{c: "scroll-view", s:[{c: "console-root scroll-content", a:{"id" : "CVUI-LogRoot"}, s:[
                                ]}, {c: "scroll-marker-v"}]}]},
                    {c: "container-back", s:[{c: "back-icon"}, {c: "content", h: "Back to main screen"}], x: "switchCategory", a: {"data-category": "quickmenu-home"}},
                    ]}
            ]}

        menu.templates["CVUILog"] = {c: "console-text", h: "[log]"}

        menu.templates["core-quickmenu"].l.push("CVUI-icon");
        menu.templates["core-quickmenu"].l.push("CVUI-menu");

        uiRef.actions["CVUI-open"] = this.actions.open;

        engine.on("CVUI-ConsoleTextUpdate", this.CVUIConsoleTextUpdate);
    },

    CVUIConsoleTextUpdate: function (logs){
        console.log("Log update")
        let root = cvr("#CVUI-LogRoot");
        root.clear();
        for (let i=0; i < logs.length; i++){
            root.appendChild(cvr.render(uiRef.templates["CVUILog"], {
                "[log]" : logs[i]
            }, uiRef.templates, uiRef.actions));
        }
    },

    actions: {
        open: function(){
            uiRef.core.playSoundCore("Click");
            uiRef.core.switchCategorySelected("CVUI");
        },
    }
}