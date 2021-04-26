extends Container


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var icon_path: String = ""
var class_selected: int = 0
var stat_edit: int = 0
var skill_list_array: Array
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func start() -> void:
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Class")
	$ClassButton.clear()
	for i in range(json_dictionary.size()):
		var class_data: Dictionary = json_dictionary["class"+String(i)]
		if i > $ClassButton.get_item_count() - 1:
			$ClassButton.add_item(class_data["name"])
		else:
			$ClassButton.set_item_text(i, class_data["name"])
	refresh_data(class_selected)

func refresh_data(id: int) -> void:
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Class")
	var class_data: Dictionary = json_dictionary["class"+String(id)]
	var class_skill_list: Dictionary = class_data["skill_list"]
	json_dictionary = get_parent().get_parent().call("read_data", "System")
	var system_stats_data: Dictionary = json_dictionary["stats"]
	json_dictionary = get_parent().get_parent().call("read_data", "Skill")
	$NameLabel/NameText.text = class_data["name"]
	var icon: String = class_data["icon"]
	if icon != "":
		$IconLabel/IconSprite.texture = load(icon)
	$ExpLabel/ExpText.text = class_data["experience"]
	$StatsLabel/StatsContainer/DataContainer/StatsListContainer/StatsList.clear()
	$StatsLabel/StatsContainer/DataContainer/FormulaListContainer/FormulaList.clear()
	for i in range(system_stats_data.size()):
		var stat_name: String = system_stats_data[String(i)]
		var class_stat_formula = class_data["stat_list"]
		var stat_formula: String = ""
		if class_stat_formula.has(stat_name):
			stat_formula = class_stat_formula[stat_name]
		else:
			stat_formula = "level * 5"
		$StatsLabel/StatsContainer/DataContainer/StatsListContainer/StatsList.add_item(stat_name)
		$StatsLabel/StatsContainer/DataContainer/FormulaListContainer/FormulaList.add_item(stat_formula)
	$SkillLabel/SkillContainer/HBoxContainer/SkillListContainer/SkillList.clear()
	$SkillLabel/SkillContainer/HBoxContainer/SkillLevelContainer/SkillLevelList.clear()
	for element in class_skill_list.keys():
		skill_list_array.append(element)
		var skill_data: Dictionary = json_dictionary["skill"+String(element)]
		var skill_name: String = skill_data["name"]
		$SkillLabel/SkillContainer/HBoxContainer/SkillListContainer/SkillList.add_item(skill_name)
		var level: String = String(class_skill_list[String(element)])
		$SkillLabel/SkillContainer/HBoxContainer/SkillLevelContainer/SkillLevelList.add_item(level)
	$SkillLabel/AddSkill/SkillLabel/OptionButton.clear()
	for element in json_dictionary.keys():
		var skill_data: Dictionary = json_dictionary[element]
		var name: String = String(skill_data["name"])
		$SkillLabel/AddSkill/SkillLabel/OptionButton.add_item(name)
		$SkillLabel/AddSkill/SkillLabel/OptionButton.select(0)
	if class_data.has("effects") == true:
		clear_effect_list()
		var effect_list: Array = class_data["effects"]
		for effect in effect_list:
			add_effect_list(effect["name"], int(effect["data_id"]), effect["value1"], effect["value2"])

func _on_Search_pressed() -> void:
	$IconLabel/IconSearch.popup_centered()

func _on_IconSearch_file_selected(path: String) -> void:
	icon_path = path
	$IconLabel/IconSprite.texture = load(path)

func _on_ClassSaveButton_pressed() -> void:
	save_class_data()
	refresh_data(class_selected)

func save_class_data() -> void:
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Class")
	var class_data: Dictionary = json_dictionary["class"+String(class_selected)]
	var class_stat_formula: Dictionary = class_data["stat_list"]
	var class_skill_list: Dictionary = class_data["skill_list"]
	var effect_list: Array
	class_skill_list.clear()
	
	class_data["name"] = $NameLabel/NameText.text
	$ClassButton.set_item_text(class_selected, $NameLabel/NameText)
	class_data["icon"] = icon_path
	class_data["experience"] = $ExpLabel/ExpText.text
	var items: int = $StatsLabel/StatsContainer/DataContainer/StatsListContainer/StatsList.get_item_count()
	for i in range(items):
		var stat: String = $StatsLabel/StatsContainer/DataContainer/StatsListContainer/StatsList.get_item_text(i)
		var formula: String = $StatsLabel/StatsContainer/DataContainer/FormulaListContainer/FormulaList.get_item_text(i)
		class_stat_formula[stat] = formula
	var skills_count = $SkillLabel/SkillContainer/HBoxContainer/SkillLevelContainer/SkillLevelList.get_item_count()
	for i in range(skills_count):
		var skill: String = String(skill_list_array[i])
		var level: int = int($SkillLabel/SkillContainer/HBoxContainer/SkillLevelContainer/SkillLevelList.get_item_text(i))
		class_skill_list[skill] = level
	class_data["stat_list"] = class_stat_formula
	class_data["skill_list"] = class_skill_list
	var effect_size: int = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.get_item_count()
	for i in range(effect_size):
		var effect_data: Dictionary
		effect_data["name"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.get_item_text(i)
		effect_data["data_id"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.get_item_text(i)
		effect_data["value1"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.get_item_text(i)
		effect_data["value2"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.get_item_text(i)
		effect_list.append(effect_data)
	class_data["effects"] = effect_list
	get_parent().get_parent().call("store_data", "Class", json_dictionary)

func _on_AddClass_pressed() -> void:
	$ClassButton.add_item("NewClass")
	var id: int = $ClassButton.get_item_count() - 1
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Class")
	var class_data: Dictionary
	var stat_data: Dictionary
	var skill_data: Dictionary
	class_data["name"] = "NewClass"
	class_data["icon"] = ""
	class_data["experience"] = "level * 30"
	stat_data["hp"] = "level * 25 + 10"
	stat_data["mp"] = "level * 15 + 5"
	stat_data["atk"] = "level * 5 + 3"
	stat_data["def"] = "level * 5 + 3"
	stat_data["int"] = "level * 5 + 3"
	stat_data["res"] = "level * 5 + 3"
	stat_data["spd"] = "level * 5 + 3"
	stat_data["luk"] = "level * 5 + 3"
	class_data["stat_list"] = stat_data
	skill_data[0] =  5
	class_data["skill_list"] = skill_data
	json_dictionary["class" + String(id)] = class_data
	get_parent().get_parent().call("store_data", "Class", json_dictionary)

func _on_RemoveClass_pressed() -> void:
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Class")
	if (json_dictionary.keys().size() > 1):
		var class_id: int = class_selected
		while class_id < (json_dictionary.keys().size() - 1):
			json_dictionary["class"+String(class_id)] = json_dictionary["class"+String(class_id+1)]
			class_id += 1
		json_dictionary.erase("class"+String(class_id))
		get_parent().get_parent().call("store_data", "Class", json_dictionary)
		$ClassButton.remove_item(class_selected)
		if class_selected == 0:
			$ClassButton.select(class_selected + 1)
			class_selected += 1
		else:
			$ClassButton.select(class_selected + 1)
			class_selected -= 1
		$ClassButton.select(class_selected)
		refresh_data(class_selected)

func _on_ClassButton_item_selected(id: int) -> void:
	class_selected = id
	refresh_data(id)

func _on_AddSkillButton_pressed() -> void:
	$SkillLabel/AddSkill.popup_centered()

func _on_RemoveButton_pressed() -> void:
	var selected: int = $SkillLabel/SkillContainer/HBoxContainer/SkillListContainer/SkillList.get_selected_items()[0]
	$SkillLabel/SkillContainer/HBoxContainer/SkillListContainer/SkillList.remove_item(selected)
	$SkillLabel/SkillContainer/HBoxContainer/SkillLevelContainer/SkillLevelList.remove_item(selected)
	skill_list_array.remove(selected)

func _on_OkButton_pressed() -> void:
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Skill")
	var skill: int = $SkillLabel/AddSkill/SkillLabel/OptionButton.get_selected_id()
	var level: int = int($SkillLabel/AddSkill/LevelLabel/LevelSpin.value)
	skill_list_array.append(String(skill))
	var skill_data: Dictionary = json_dictionary["skill"+String(skill)]
	var skill_name: String = skill_data["name"]
	$SkillLabel/SkillContainer/HBoxContainer/SkillListContainer/SkillList.add_item(skill_name)
	$SkillLabel/SkillContainer/HBoxContainer/SkillLevelContainer/SkillLevelList.add_item(String(level))
	$SkillLabel/AddSkill.hide()

func _on_CancelButton_pressed() -> void:
	$SkillLabel/AddSkill.hide()

func _on_StatsList_item_selected(index: int) -> void:
	$StatsLabel/StatsContainer/DataContainer/FormulaListContainer/FormulaList.select(index)

func _on_FormulaList_item_selected(index: int) -> void:
	$StatsLabel/StatsContainer/DataContainer/StatsListContainer/StatsList.select(index)

func _on_SkillList_item_selected(index: int) -> void:
	$SkillLabel/SkillContainer/HBoxContainer/SkillLevelContainer/SkillLevelList.select(index)

func _on_SkillLevelList_item_selected(index: int) -> void:
	$SkillLabel/SkillContainer/HBoxContainer/SkillListContainer/SkillList.select(index)

func _on_FormulaList_item_activated(index: int) -> void:
	var stat_name: String = $StatsLabel/StatsContainer/DataContainer/StatsListContainer/StatsList.get_item_text(index)
	var stat_formula: String = $StatsLabel/StatsContainer/DataContainer/FormulaListContainer/FormulaList.get_item_text(index)
	$StatEditor/StatLabel.text = stat_name
	$StatEditor/StatEdit.text = stat_formula
	stat_edit = index
	$StatEditor.show()

func _on_OkStatButton_pressed() -> void:
	var stat_formula: String = $StatEditor/StatEdit.text
	$StatsLabel/StatsContainer/DataContainer/FormulaListContainer/FormulaList.set_item_text(stat_edit, stat_formula)
	save_class_data()
	stat_edit = -1
	$StatEditor.hide()

func _on_CancelStatButton_pressed() -> void:
	stat_edit = -1
	$StatEditor.hide()

func _on_AddClassEffect_pressed() -> void:
	get_parent().get_parent().call("open_effect_manager", "Class")

func _on_RemoveClassEffect_pressed() -> void:
	var id: int = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.get_selected_items()[0]
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.remove_item(id)

func add_effect_list(name: String, data_id: int, value1: String, value2: String) -> void:
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.add_item(name)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.add_item(data_id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.add_item(value1)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.add_item(value2)

func clear_effect_list() -> void:
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.clear()
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.clear()
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.clear()
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.clear()

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
